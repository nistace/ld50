using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Utils.Audio;

namespace Ld50 {
	public class EnemyShip : MonoBehaviour {
		[SerializeField] protected Vector3          _spawn       = new Vector3(-20, -.5f, 0);
		[SerializeField] protected Vector3          _destination = new Vector3(20, .5f, 0);
		[SerializeField] protected float            _speed       = 1;
		[SerializeField] protected EnemyShipCanon[] _canons;
		[SerializeField] protected float            _canonShootDelay = .1f;
		[SerializeField] protected float            _hitDelay;
		[SerializeField] protected SpriteRenderer   _renderer;
		[SerializeField] protected Sprite[]         _sprites;
		[SerializeField] protected float            _animationSpeed = 10f;

		public static UnityEvent onHitShot { get; } = new UnityEvent();

		public IEnumerator Play(float shootLerp, int hits = 1) {
			transform.position = _spawn;
			var shot = false;
			var xShoot = Mathf.Lerp(_spawn.x, _destination.x, shootLerp);
			while (transform.position != _destination) {
				_renderer.sprite = _sprites[(int)(Time.time * _animationSpeed) % _sprites.Length];
				transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
				if (!shot && Mathf.Abs(xShoot - transform.position.x) < 1) {
					StartCoroutine(Shoot(hits));
					shot = true;
				}
				yield return null;
			}
		}

		private IEnumerator Shoot(int hits) {
			foreach (var canon in _canons) {
				StartCoroutine(canon.Fire());
				yield return new WaitForSeconds(_canonShootDelay);
			}
			yield return new WaitForSeconds(_hitDelay);
			for (var i = 0; i < _canons.Length; ++i) {
				if (hits > Random.Range(0, _canons.Length - i)) {
					AudioManager.Sfx.PlayRandom("hit");
					hits--;
					onHitShot.Invoke();
				}
				else if (Random.value < .3f) {
					AudioManager.Sfx.PlayRandom("missed");
				}
				yield return new WaitForSeconds(_canonShootDelay);
			}
		}
	}
}