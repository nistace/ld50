using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace Ld50.Ships {
	public class ShipTaskManager : MonoBehaviour {
		[SerializeField]                        protected bool           _running;
		[SerializeField]                        protected float          _runningTime;
		[SerializeField]                        protected ShipNodeTask   _taskNodePrefab;
		[Header("Morale"), SerializeField]      protected float          _minMorale      = .5f;
		[SerializeField]                        protected float          _maxMorale      = 1.3f;
		[SerializeField]                        protected float          _baseMorale     = 1;
		[SerializeField]                        protected float          _moraleBonus    = .01f;
		[Header("Ship Speed"), SerializeField]  protected float          _minShipSpeed   = 5f;
		[SerializeField]                        protected float          _maxShipSpeed   = 25f;
		[SerializeField]                        protected float          _baseShipSpeed  = 5f;
		[SerializeField]                        protected float          _shipSpeedBonus = 1f;
		[SerializeField]                        protected float          _distanceTravelled;
		[SerializeField]                        protected float          _initialDistanceToDestination = 1500000;
		[Header("Enemy Ships"), SerializeField] protected float          _enemyShipDelay               = 30f;
		[SerializeField]                        protected float          _detectionDelayMultiplier     = 1.5f;
		[SerializeField]                        protected float          _nextEnemyShipCooldown;
		[Header("Planks"), SerializeField]      protected int            _basePlankCount = 4;
		[SerializeField]                        protected ShipFishingRod _rod;
		[Header("Sinking"), SerializeField]     protected float          _sinkingPerHole = .01f;
		[SerializeField]                        protected float          _pumpBoost      = .02f;

		[SerializeField] protected List<ShipNodeTask> _tasks = new List<ShipNodeTask>();

		public float runningTime           => _runningTime;
		public float morale                { get; private set; }
		public float minMorale             => _minMorale;
		public float maxMorale             => _maxMorale;
		public float shipSpeed             { get; private set; }
		public float minShipSpeed          => _minShipSpeed;
		public float maxShipSpeed          => _maxShipSpeed;
		public float enemyShipRealDelay    => _nextEnemyShipCooldown * (shipDetectionEnabled ? Mathf.Max(morale * _detectionDelayMultiplier, 1) : 1);
		public bool  shipDetectionEnabled  { get; private set; }
		public int   plankCount            { get; private set; }
		public float sinkingSpeed          { get; private set; }
		public float distanceTravelled     => _distanceTravelled;
		public float distanceToDestination => _initialDistanceToDestination - distanceTravelled;

		public static UnityEvent         onEnemyShipDelayReachedZero { get; } = new UnityEvent();
		public static ShipNodeTask.Event onNewTask                   { get; } = new ShipNodeTask.Event();

		private void Start() {
			FishingPlankManager.onPlankFished.AddListenerOnce(() => plankCount++);
		}

		public void ResetValues() {
			_running = false;
			morale = _baseMorale;
			shipSpeed = _baseShipSpeed;
			_nextEnemyShipCooldown = _enemyShipDelay;
			plankCount = _basePlankCount;
			_distanceTravelled = 0;
		}

		private void Update() {
			if (!_running) return;
			_runningTime += Time.deltaTime;
			UpdateCrewMorale();
			UpdateFishing();
			UpdateShipSpeed();
			UpdateEnemyShipDelay();
			UpdateSinking();
		}

		private void UpdateFishing() {
			_rod.SetInUse(_tasks.Any(t => t.type == ShipNodeTask.Type.FishPlanks && t.status == ShipNodeTask.Status.InProgress));
		}

		private void UpdateSinking() {
			sinkingSpeed = _tasks.Count(t => t.type == ShipNodeTask.Type.Repair && t.status != ShipNodeTask.Status.Completed) * _sinkingPerHole;
			sinkingSpeed -= _tasks.Count(t => t.type == ShipNodeTask.Type.Pump && t.status == ShipNodeTask.Status.InProgress) * _pumpBoost * morale;
		}

		private void UpdateEnemyShipDelay() {
			shipDetectionEnabled = _tasks.Any(t => t.type == ShipNodeTask.Type.LookOut && t.status == ShipNodeTask.Status.InProgress);
			_nextEnemyShipCooldown -= Time.deltaTime / (shipDetectionEnabled ? Mathf.Max(morale * _detectionDelayMultiplier, 1) : 1);
			if (_nextEnemyShipCooldown <= 0) {
				_nextEnemyShipCooldown = _enemyShipDelay;
				onEnemyShipDelayReachedZero.Invoke();
			}
		}

		private void UpdateShipSpeed() {
			var shipSpeedInfluence = _tasks.Count(t => t.type == ShipNodeTask.Type.Tiller && t.status == ShipNodeTask.Status.InProgress);
			shipSpeed = shipSpeedInfluence > 0
				? Mathf.Clamp(shipSpeed + morale * shipSpeedInfluence * Time.deltaTime * _shipSpeedBonus, _minShipSpeed, _maxShipSpeed)
				: Mathf.Clamp(shipSpeed - Time.deltaTime * _shipSpeedBonus, _minShipSpeed, _maxShipSpeed);
			_distanceTravelled += shipSpeed * Time.deltaTime;
		}

		private void UpdateCrewMorale() {
			var moraleInfluence = _tasks.Count(t => t.type == ShipNodeTask.Type.Accordion && t.status == ShipNodeTask.Status.InProgress);
			morale = moraleInfluence > 0
				? Mathf.Clamp(morale + morale * moraleInfluence * Time.deltaTime * _moraleBonus, _minMorale, _maxMorale)
				: Mathf.Clamp(morale - Time.deltaTime * _moraleBonus, _minMorale, _maxMorale);
		}

		public ShipNodeTask CreateNewTask(ShipNodeTask.Type type, Vector2 localPosition) {
			var newTask = Instantiate(_taskNodePrefab, transform);
			newTask.transform.localPosition = localPosition;
			var interactionText = newTask.interactionTexts.Where(t => _tasks.All(u => u.interactableText != t)).Random();
			newTask.Reset(type, interactionText);
			_tasks.Add(newTask);
			newTask.onCompleted.AddListenerOnce(() => _tasks.Remove(newTask));
			onNewTask.Invoke(newTask);
			return newTask;
		}

		[ContextMenu("Find All Tasks")] private void FindAllTasks() => _tasks = FindObjectsOfType<ShipNodeTask>(true).ToList();
		public IEnumerable<ShipNodeTask> GetAllAvailableTasks() => _tasks.Where(t => t.type != ShipNodeTask.Type.None && t.status == ShipNodeTask.Status.Waiting);

		public void ConsumePlank() => plankCount--;

		public void Run() {
			_running = true;
			_runningTime = 0;
		}

		public void Stop() {
			_running = false;
		}
	}
}