using System;
using Ld50.Scenes.Game.Ui;
using Ld50.Ships;
using Ld50.Text;
using UnityEngine;
using Utils.Audio;
using Utils.Extensions;
using Utils.Libraries;
using Random = UnityEngine.Random;

namespace Ld50.Scenes.Game {
	public class GameManager : MonoBehaviour {
		[SerializeField] protected Pirate    _captain;
		[SerializeField] protected Pirate[]  _pirates;
		[SerializeField] protected EnemyShip _enemyShip;
		[SerializeField] protected int       _maxHitsPerEnemyShip = 8;
		[SerializeField] protected float     _difficulty          = .1f;

		private Pirate instructionTarget { get; set; }

		private void Start() {
			Ship.pathSystem.Initialize();
			Ship.taskManager.ResetValues();
			Ship.taskManager.Run();
			ShipTaskManager.onEnemyShipDelayReachedZero.AddListenerOnce(SpawnEnemyShip);
			EnemyShip.onHitShot.AddListenerOnce(Ship.AddHole);
			Pirate.onImpossibleTask.AddListenerOnce(HandlePirateHasImpossibleTask);
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
				Debug.Log("Changes of " + i + " hits:" + i / (_difficulty * sqrtTime));
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
			if (Ship.y < Ship.gameOverY) {
				Ship.taskManager.Stop();
				Debug.Log($"Game Over, distance travelled: {Ship.taskManager.distanceTravelled:0.0}m");
			}
		}
	}
}