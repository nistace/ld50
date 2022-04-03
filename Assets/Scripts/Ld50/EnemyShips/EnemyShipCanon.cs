using System.Collections;
using UnityEngine;
using Utils.Audio;

namespace Ld50 {
	public class EnemyShipCanon : MonoBehaviour {
		[SerializeField] protected SpriteRenderer _renderer;
		[SerializeField] protected Sprite[]       _fireSprites;
		[SerializeField] protected float          _animationSpeed = .1f;

		public IEnumerator Fire() {
			_renderer.enabled = true;
			AudioManager.Sfx.PlayRandom("canon");
			foreach (var sprite in _fireSprites) {
				_renderer.sprite = sprite;
				yield return new WaitForSeconds(_animationSpeed);
			}
			_renderer.enabled = false;
		}
	}
}