using System;
using Ld50;
using Ld50.Ships;
using UnityEngine;

public class Pirate : MonoBehaviour {
	[SerializeField] protected ShipPathNode   _initialNode;
	[SerializeField] protected ShipPathNode   _destination;
	[SerializeField] protected ShipPath       _path;
	[SerializeField] protected PirateRenderer _renderer;
	[SerializeField] protected float          _speed = 4;

	private Vector2 position {
		get => transform.position;
		set => transform.position = value;
	}

	private void Start() {
		position = _initialNode.position;
		_renderer.animation = PirateAnimationSet.Animation.Idle;
	}

	[ContextMenu("Go To destination")]
	private void GoToDestination() {
		if (Ship.pathSystem.TryGetPath(_path?.currentNode ?? _initialNode, _destination, out var path)) {
			FollowPath(path);
		}
	}

	public void FollowPath(ShipPath path) => _path = path;

	private void Update() {
		if (_path == null || _path.destinationReached) return;

		if (_path.IsAtNextNode(position)) {
			_path.ProgressToNextNode();
			if (_path.destinationReached) {
				_renderer.animation = PirateAnimationSet.Animation.Idle;
				return;
			}
		}

		_renderer.lookLeft = _path.nextNodeLocalPosition.x < position.x;
		_renderer.animation = GetLinkAnimation(_path.nextNodeLinkType);
		position = Vector2.MoveTowards(position, _path.nextNodeLocalPosition, _speed * Time.deltaTime);
	}

	private static PirateAnimationSet.Animation GetLinkAnimation(ShipPathLink.LinkType linkType) {
		switch (linkType) {
			case ShipPathLink.LinkType.Walk: return PirateAnimationSet.Animation.Walk;
			case ShipPathLink.LinkType.Climb: return PirateAnimationSet.Animation.Climb;
			default: throw new ArgumentOutOfRangeException(nameof(linkType), linkType, null);
		}
	}
}