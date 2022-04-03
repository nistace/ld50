using System;
using Ld50.Text;
using UnityEngine;

namespace Ld50.Ships {
	[RequireComponent(typeof(ShipPathNode))]
	public class ShipNodeTask : MonoBehaviour, ITextInteractable {
		[SerializeField] protected ShipPathNode _pathNode;
		[SerializeField] protected Type         _type;
		[SerializeField] protected Status       _status = Status.Waiting;
		[SerializeField] protected string       _interactionText;
		[SerializeField] protected Sprite       _interactionIdeaSprite;
		[SerializeField] protected float        _progressSpeed;
		[SerializeField] protected float        _progress;

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

		public ShipPathNode pathNode               => _pathNode ? _pathNode : _pathNode = GetComponent<ShipPathNode>();
		public Type         type                   => _type;
		public Status       status                 => _status;
		public string       interactableText       => _interactionText;
		public Sprite       interactableIdeaSprite => _interactionIdeaSprite;

		private void Reset() => Reset(type);

		public void Reset(Type type) {
			_type = type;
			_pathNode = pathNode;
			_status = Status.Waiting;
		}

		public void Claim() => _status = Status.Claimed;
		public void Quit() => _status = Status.Waiting;

		public void Progress() {
			_status = Status.InProgress;
			_progress += _progressSpeed * Time.deltaTime;
			if (_progress >= 1) _status = Status.Completed;
		}
	}
}