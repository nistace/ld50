using System;
using Ld50.Ships;
using UnityEngine;

public class SeaBackground : MonoBehaviour {
	[SerializeField] protected RendererMovement[] _rendererMovements;

	private void Update() {
		foreach (var rendererMovement in _rendererMovements) rendererMovement.Refresh();
	}

	[Serializable]
	public class RendererMovement {
		[SerializeField] protected SpriteRenderer _renderer;
		[SerializeField] protected Vector2        _minSize     = new Vector2(32, 32);
		[SerializeField] protected float          _speed       = .1f;
		[SerializeField] protected float          _loopingSize = 32;

		public void Refresh() => _renderer.size = _minSize + new Vector2((Ship.taskManager.distanceTravelled * _speed) % _loopingSize, 0);
	}
}