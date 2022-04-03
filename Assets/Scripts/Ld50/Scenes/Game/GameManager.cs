using System;
using Ld50.Scenes.Game.Ui;
using Ld50.Ships;
using Ld50.Text;
using UnityEngine;
using Utils.Extensions;
using Random = UnityEngine.Random;

namespace Ld50.Scenes.Game {
	public class GameManager : MonoBehaviour {
		[SerializeField] protected Pirate    _captain;
		[SerializeField] protected float     _gameOverShipY = -4.5f;
		[SerializeField] protected Pirate[]  _pirates;
		[SerializeField] protected EnemyShip _enemyShip;

		private Pirate instructionTarget { get; set; }

		private void Start() {
			Ship.pathSystem.Initialize();
			Ship.taskManager.ResetValues();
			Ship.taskManager.running = true;
			ShipTaskManager.onEnemyShipDelayReachedZero.AddListenerOnce(SpawnEnemyShip);
			EnemyShip.onHitShot.AddListenerOnce(HandleHit);
			TextInputManager.listening = true;
			ListenPirateName();
		}

		private static void HandleHit() {
			if (Ship.hull.TryAddHole(out var newHolePosition)) {
				Ship.taskManager.CreateNewTask(ShipNodeTask.Type.Repair, newHolePosition);
			}
		}

		private void SpawnEnemyShip() => StartCoroutine(_enemyShip.Play(Random.Range(.1f, .8f)));

		private void ListenPirateName() {
			instructionTarget = null;
			TextInputManager.SetOptions(_pirates);
			InteractionManagerUi.HideAll();
			InteractionManagerUi.ShowAll(TextInputManager.allCurrentOptions);
			InteractionManagerUi.HideMarker();
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
		}

		private void HandleSelectedInstruction(ITextInteractable interactable) {
			TextInputManager.onInteractableSpelled.RemoveListener(HandleSelectedInstruction);
			var workNode = interactable as ShipNodeTask ?? throw new ArgumentException($"Was expecting a task but got {interactable.interactableText}");
			instructionTarget.Reassign(workNode);
			InteractionManagerUi.HideMarker();
			ListenPirateName();
		}

		private void Update() {
			if (Ship.y < _gameOverShipY) {
				Debug.Log($"Game Over, distance travelled: {Ship.taskManager.distanceTravelled:0.0}m");
			}
		}
	}
}