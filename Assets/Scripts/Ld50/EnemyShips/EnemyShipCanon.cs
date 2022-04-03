using System.Collections;
using UnityEngine;

namespace Ld50 {
	public class EnemyShipCanon : MonoBehaviour {
		[SerializeField] protected SpriteRenderer _renderer;
		[SerializeField] protected Sprite[]       _fireSprites;
		[SerializeField] protected AudioSource    _source;
		[SerializeField] protected float          _animationSpeed = .1f;

		public IEnumerator Fire() {
			_renderer.enabled = true;
			if (_source) _source.Play();
			foreach (var sprite in _fireSprites) {
				_renderer.sprite = sprite;
				yield return new WaitForSeconds(_animationSpeed);
			}
			_renderer.enabled = false;
		}
	}
}