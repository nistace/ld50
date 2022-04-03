using System;
using Ld50;
using Ld50.Ships;
using Ld50.Text;
using UnityEngine;

public class Pirate : MonoBehaviour, ITextInteractable {
	[SerializeField] protected ShipPathNode   _initialNode;
	[SerializeField] protected ShipNodeTask   _assignment;
	[SerializeField] protected ShipPath       _path;
	[SerializeField] protected PirateRenderer _renderer;
	[SerializeField] protected float          _speed = 4;

	public string  interactableText       => name;
	public Sprite  interactableIdeaSprite => _renderer.GetDefaultSprite();
	public Vector2 worldPosition          => position;

	private Vector2 position {
		get => transform.position;
		set => transform.position = value;
	}

	private void Start() {
		if (_initialNode.TryGetComponent<ShipNodeTask>(out var nodeTask)) Reassign(nodeTask);
		position = _initialNode.position;
	}

	public void Reassign(ShipNodeTask newAssignment) {
		CancelAssignment();
		if (newAssignment && Ship.pathSystem.TryGetPath(_path?.currentNode ?? _initialNode, newAssignment.pathNode, out var path)) {
			_assignment = newAssignment;
			_assignment.Claim();
			_path = path;
		}
	}

	public void CancelAssignment() {
		if (_assignment) {
			_assignment.Quit();
			_assignment = null;
			_renderer.animation = PirateAnimationSet.Animation.Idle;
		}
	}

	private void Update() {
		if (!_assignment) return;
		if (_path == null) return;
		if (_path.destinationReached) {
			_assignment.Progress();
			return;
		}

		if (_path.IsAtNextNode(position)) {
			_path.ProgressToNextNode();
			if (_path.destinationReached) {
				_renderer.lookLeft = false;
				_renderer.animation = GetTaskAnimation(_assignment.type);
				return;
			}
		}

		_renderer.lookLeft = _path.nextNodeLocalPosition.x < position.x;
		_renderer.animation = GetLinkAnimation(_path.nextNodeLinkType);
		position = Vector2.MoveTowards(position, _path.nextNodeLocalPosition, _speed * Time.deltaTime);
	}

	private static PirateAnimationSet.Animation GetTaskAnimation(ShipNodeTask.Type shipTaskType) {
		switch (shipTaskType) {
			case ShipNodeTask.Type.None: return PirateAnimationSet.Animation.Idle;
			case ShipNodeTask.Type.Tiller: return PirateAnimationSet.Animation.Till;
			case ShipNodeTask.Type.FishPlanks: return PirateAnimationSet.Animation.FishPlanks;
			case ShipNodeTask.Type.LookOut: return PirateAnimationSet.Animation.LookOut;
			case ShipNodeTask.Type.Pump: return PirateAnimationSet.Animation.Idle;
			case ShipNodeTask.Type.Repair: return PirateAnimationSet.Animation.Repair;
			case ShipNodeTask.Type.Accordion: return PirateAnimationSet.Animation.PlayAccordion;
			default: throw new ArgumentOutOfRangeException(nameof(shipTaskType), shipTaskType, null);
		}
	}

	private static PirateAnimationSet.Animation GetLinkAnimation(ShipPathLink.LinkType linkType) {
		switch (linkType) {
			case ShipPathLink.LinkType.Walk: return PirateAnimationSet.Animation.Walk;
			case ShipPathLink.LinkType.Climb: return PirateAnimationSet.Animation.Climb;
			default: throw new ArgumentOutOfRangeException(nameof(linkType), linkType, null);
		}
	}
}