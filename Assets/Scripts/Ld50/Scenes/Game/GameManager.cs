using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Ld50.Scenes.CommonUi;
using Ld50.Scenes.Game.Ui;
using Ld50.Ships;
using Ld50.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Audio;
using Utils.Extensions;
using Utils.Libraries;
using Web;
using Random = UnityEngine.Random;

namespace Ld50.Scenes.Game {
	public class GameManager : MonoBehaviour {
		[SerializeField] protected Pirate    _captain;
		[SerializeField] protected Pirate[]  _pirates;
		[SerializeField] protected EnemyShip _enemyShip;
		[SerializeField] protected int       _maxHitsPerEnemyShip = 8;
		[SerializeField] protected float     _difficulty          = .1f;
		[SerializeField] protected GameUi    _ui;
		[SerializeField] protected string[]  _gameOverSayings;

		private Pirate instructionTarget { get; set; }
		private bool   gameOver          { get; set; }

		private void Start() => StartCoroutine(FadeInAndInitialize());

		private IEnumerator FadeInAndInitialize() {
			Ship.pathSystem.Initialize();
			Ship.taskManager.ResetValues();
			ShipTaskManager.onEnemyShipDelayReachedZero.AddListenerOnce(SpawnEnemyShip);
			EnemyShip.onHitShot.AddListenerOnce(Ship.AddHole);
			Pirate.onImpossibleTask.AddListenerOnce(HandlePirateHasImpossibleTask);
			InteractionManagerUi.HideMarker();
			InteractionManagerUi.HideAll();

			yield return StartCoroutine(TransitionScreenUi.Fade(0, 1));

			Ship.taskManager.Run();
			TextInputManager.listening = true;
			ListenPirateName();
		}

		private static void HandlePirateHasImpossibleTask(Pirate pirate, Pirate.ImpossibleTaskReason reason) =>
			SpeechBubbleManagerUi.ShowIdea(pirate.transform, Sprites.Of($"pirate.impossibleTask.{reason}"));

		[ContextMenu("Add Hole")] private void AddHole() => Ship.AddHole();

		private void SpawnEnemyShip() => StartCoroutine(_enemyShip.Play(Random.value < .5f ? .2f : .75f, GetRandomHitsByEnemyShip()));

		private int GetRandomHitsByEnemyShip() {
			var sqrtTime = Mathf.Sqrt(Ship.taskManager.runningTime);
			for (var i = 1; i < _maxHitsPerEnemyShip; ++i) {
				if (Random.value < i / (_difficulty * sqrtTime)) {
					return i;
				}
			}
			return _maxHitsPerEnemyShip;
		}

		private void ListenPirateName() {
			instructionTarget = null;
			TextInputManager.SetOptions(_pirates);
			InteractionManagerUi.HideAll();
			InteractionManagerUi.ShowAll(TextInputManager.allCurrentOptions);
			InteractionManagerUi.HideMarker();
			TextInputManager.onNewLetterEntered.AddListenerOnce(() => AudioManager.Voices.PlayRandom("captain.noise"));
			TextInputManager.onInteractableSpelled.AddListenerOnce(HandleSelectedPirate);
		}

		private void HandleSelectedPirate(ITextInteractable interactable) {
			TextInputManager.onInteractableSpelled.RemoveListener(HandleSelectedPirate);
			instructionTarget = interactable as Pirate ?? throw new ArgumentException($"Was expecting a pirate but got {interactable.interactableText}");
			instructionTarget.CancelAssignment();
			InteractionManagerUi.ShowMarker(instructionTarget.transform);
			ListenInstruction();
		}

		private void ListenInstruction() {
			TextInputManager.SetOptions(Ship.taskManager.GetAllAvailableTasks());
			InteractionManagerUi.HideAll();
			InteractionManagerUi.ShowAll(TextInputManager.allCurrentOptions);
			TextInputManager.onInteractableSpelled.AddListenerOnce(HandleSelectedInstruction);
			ShipTaskManager.onNewTask.AddListenerOnce(AddPossibleInstruction);
		}

		private static void AddPossibleInstruction(ShipNodeTask task) => InteractionManagerUi.Show(TextInputManager.AddOption(task));

		private void HandleSelectedInstruction(ITextInteractable interactable) {
			TextInputManager.onInteractableSpelled.RemoveListener(HandleSelectedInstruction);
			ShipTaskManager.onNewTask.RemoveListener(AddPossibleInstruction);
			var workNode = interactable as ShipNodeTask ?? throw new ArgumentException($"Was expecting a task but got {interactable.interactableText}");
			instructionTarget.Reassign(workNode);
			InteractionManagerUi.HideMarker();
			ListenPirateName();
		}

		private void Update() {
			if (gameOver) return;
			if (Ship.y < Ship.gameOverY) {
				StartCoroutine(PlayGameOver());
			}
		}

		private IEnumerator PlayGameOver() {
			gameOver = true;
			TextInputManager.listening = false;
			Ship.taskManager.Stop();
			Ship.ForceToSink();
			foreach (var pirate in _pirates) pirate.SwimToTheSurface();
			_captain.SwimToTheSurface();

			TextInputManager.onInteractableSpelled.RemoveAllListeners();
			ShipTaskManager.onNewTask.RemoveAllListeners();
			InteractionManagerUi.HideAll();
			Debug.Log($"Game Over, distance travelled: {Ship.taskManager.distanceTravelled:0.0}m");
			while (!_captain.IsSwimmingAtTheSurface()) yield return null;
			while (_pirates.Any(t => !t.IsSwimmingAtTheSurface())) yield return null;
			_ui.stats.Close();
			yield return new WaitForSeconds(3f);
			var saying = _gameOverSayings.Random();
			foreach (var c in saying) {
				SpeechBubbleManagerUi.ShowLetter(_captain.transform, c);
				AudioManager.Voices.PlayRandom("captain.noise");
				yield return new WaitForSeconds(.5f);
			}
			yield return new WaitForSeconds(1f);

			PostScoreResult sendScoreResult = null;
			Leaderboard leaderboard = null;
			yield return StartCoroutine(WebManager.SendScore(Pirate.captainsName, (int)Ship.taskManager.distanceTravelled, t => sendScoreResult = t));
			yield return StartCoroutine(WebManager.GetLeaderBoard(t => leaderboard = t));

			var ranking = leaderboard.result.IndexWhere(t => t.id == sendScoreResult.id);
			var previous = ranking == 0 ? null : leaderboard.result[0];
			_ui.gameOver.Show((int)Ship.taskManager.runningTime, (int)Ship.taskManager.distanceTravelled, FormatBigNumber((int)Ship.taskManager.distanceToDestination), ranking,
				previous == null ? string.Empty : $"{previous.name} was able to travel {previous.score} meters");

			_ui.gameOver.onContinueClicked.AddListenerOnce(GoToMenu);
			yield return null;
		}

		private void GoToMenu() => StartCoroutine(TransitionToMenu());

		private IEnumerator TransitionToMenu() {
			yield return StartCoroutine(TransitionScreenUi.Fade(1, 1));
			SceneManager.LoadScene("Menu");
		}

		private static string FormatBigNumber(int number) {
			var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
			nfi.NumberGroupSeparator = " ";
			return number.ToString("#,0", nfi);
		}
	}
}