using Ld50.Text;
using UnityEngine;
using UnityEngine.Events;
using Utils.Audio;

namespace Ld50.Ships {
	[RequireComponent(typeof(ShipPathNode))]
	public class ShipNodeTask : MonoBehaviour, ITextInteractable {
		public class Event : UnityEvent<ShipNodeTask> { }

		[SerializeField] protected ShipPathNode _pathNode;
		[SerializeField] protected Type         _type;
		[SerializeField] protected Status       _status = Status.Waiting;
		[SerializeField] protected string       _interactionText;
		[SerializeField] protected Sprite       _interactionIdeaSprite;
		[SerializeField] protected float        _progressSpeed;
		[SerializeField] protected float        _progress;
		[SerializeField] protected float        _sfxFrequency = -1;
		[SerializeField] protected bool         _plankRequired;
		[SerializeField] protected string[]     _interactionTexts;

		public enum Type {
			None       = 0,
			Tiller     = 1,
			FishPlanks = 2,
			LookOut    = 3,
			Pump       = 4,
			Repair     = 5,
			Accordion  = 6
		}

		public enum Status {
			Waiting    = 0,
			Claimed    = 1,
			InProgress = 2,
			Completed  = 3
		}

		public  ShipPathNode pathNode               => _pathNode ? _pathNode : _pathNode = GetComponent<ShipPathNode>();
		public  Type         type                   => _type;
		public  Status       status                 => _status;
		public  string       interactableText       => _interactionText;
		public  Sprite       interactableIdeaSprite => _interactionIdeaSprite;
		public  bool         plankRequired          => _plankRequired;
		private float        nextSfxTime            { get; set; }
		public  bool         hasProgress            => _progress > 0f;
		public  string[]     interactionTexts       => _interactionTexts;

		public UnityEvent onCompleted { get; } = new UnityEvent();

		private void Reset() => Reset(type, string.Empty);

		public void Reset(Type type, string interactionText) {
			_type = type;
			_pathNode = pathNode;
			_status = Status.Waiting;
			_interactionText = interactionText;
		}

		public void Claim() => _status = Status.Claimed;
		public void Quit() => _status = Status.Waiting;

		public void Progress() {
			if (_status == Status.Completed) return;
			_status = Status.InProgress;
			_progress += Ship.taskManager.morale * _progressSpeed * Time.deltaTime;
			if (_sfxFrequency > 0 && Time.time > nextSfxTime) {
				AudioManager.Sfx.PlayRandom($"{type}");
				nextSfxTime = Time.time + 1 / _sfxFrequency;
			}
			if (_progress >= 1) {
				_status = Status.Completed;
				onCompleted.Invoke();
			}
		}
	}
}