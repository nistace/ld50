using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace Ld50.Ships {
	public class FishingPlankManager : MonoBehaviour {
		[Header("Rod"), SerializeField]    protected SpriteRenderer _hook;
		[SerializeField]                   protected float          _hookSpeed = 2;
		[SerializeField]                   protected SpriteRenderer _line;
		[SerializeField]                   protected ShipFishingRod _rod;
		[Header("Planks"), SerializeField] protected Transform      _plankPrefab;
		[SerializeField]                   protected float          _plankSpeed = .5f;
		[SerializeField]                   protected float          _spawnX;
		[SerializeField]                   protected float          _destinationX;
		[SerializeField]                   protected float          _verticalMovement      = .2f;
		[SerializeField]                   protected float          _verticalMovementSpeed = .2f;
		[SerializeField]                   protected float          _nextPlankMinDelay     = 4;
		[SerializeField]                   protected float          _nextPlankMaxDelay     = 8;

		private float     nextPlankSpawnTime { get; set; }
		private Transform fishedPlank        { get; set; }

		private Queue<Transform> pooledPlanks   { get; } = new Queue<Transform>();
		private List<Transform>  floatingPlanks { get; } = new List<Transform>();

		public static UnityEvent onPlankFished { get; } = new UnityEvent();

		private void Start() => nextPlankSpawnTime = Time.time + Random.Range(_nextPlankMinDelay, _nextPlankMaxDelay);

		private void Update() {
			UpdateFloatingPlanks();
			UpdateRod();
		}

		private void UpdateFloatingPlanks() {
			var y = _verticalMovement * Mathf.Sin(Time.time * _verticalMovementSpeed);
			foreach (var floatingPlank in floatingPlanks) {
				floatingPlank.transform.position = new Vector3(Mathf.MoveTowards(floatingPlank.transform.position.x, _destinationX, _plankSpeed * Time.deltaTime), y);
			}

			if (floatingPlanks.Count > 0 && Mathf.Abs(floatingPlanks[0].transform.position.x - _destinationX) < .1f) {
				PoolPlank(floatingPlanks[0]);
			}

			if (Time.time >= nextPlankSpawnTime) {
				var newPlank = pooledPlanks.Count > 0 ? pooledPlanks.Dequeue() : Instantiate(_plankPrefab, transform);
				newPlank.position = new Vector3(_spawnX, 0, 0);
				newPlank.gameObject.SetActive(true);
				floatingPlanks.Add(newPlank);
				nextPlankSpawnTime = Time.time + Random.Range(_nextPlankMinDelay, _nextPlankMaxDelay);
			}
		}

		private void UpdateRod() {
			if (_rod.inUse) {
				_hook.enabled = true;
				_line.enabled = true;
				if (fishedPlank) {
					_hook.transform.position = Vector3.MoveTowards(_hook.transform.position, _rod.tip.position, Ship.taskManager.morale * _hookSpeed * Time.deltaTime);
					if (Vector3.SqrMagnitude(_hook.transform.position - _rod.tip.position) < .1f) {
						PoolPlank(fishedPlank);
						onPlankFished.Invoke();
					}
				}
				else if (_hook.transform.localPosition != Vector3.zero) {
					_hook.transform.localPosition = Vector3.MoveTowards(_hook.transform.localPosition, Vector3.zero, Ship.taskManager.morale * _hookSpeed * Time.deltaTime);
				}
				else if (floatingPlanks.TryFirst(t => Vector3.SqrMagnitude(_hook.transform.position - t.position) < .1f, out var grabbedPlank)) {
					fishedPlank = grabbedPlank;
					floatingPlanks.Remove(grabbedPlank);
					grabbedPlank.SetParent(_hook.transform);
					grabbedPlank.localPosition = Vector3.zero;
				}
				var lineVector = (Vector2)(_rod.tip.position - _line.transform.position);
				_line.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, lineVector));
				_line.size = new Vector2(1, lineVector.magnitude);
			}
			else {
				_hook.transform.position = _rod.tip.transform.position;
				if (fishedPlank) PoolPlank(fishedPlank);
				_hook.enabled = false;
				_line.enabled = false;
			}
		}

		private void PoolPlank(Transform plank) {
			pooledPlanks.Enqueue(plank);
			plank.SetParent(transform);
			plank.gameObject.SetActive(false);
			floatingPlanks.Remove(plank);
			if (plank == fishedPlank) fishedPlank = null;
		}
	}
}