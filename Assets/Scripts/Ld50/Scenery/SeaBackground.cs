using System;
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
		[SerializeField] protected Vector2        _speed       = new Vector2(1, 0);
		[SerializeField] protected Vector2        _loopingSize = new Vector2(32, 32);

		public void Refresh() => _renderer.size = _minSize + new Vector2(_speed.x * Time.time % _loopingSize.x, _speed.y * Time.time % _loopingSize.y);
	}
}