using UnityEngine;

public class SimpleAnimation : MonoBehaviour {
	[SerializeField] protected SpriteRenderer _renderer;
	[SerializeField] protected Sprite[]       _sprites;
	[SerializeField] protected float          _speed = 1;

	private void Update() {
		_renderer.sprite = _sprites[(int)(Time.time * _speed) % _sprites.Length];
	}
}