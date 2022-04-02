using UnityEngine;

namespace Ld50.Ships {
	public class Ship : MonoBehaviour {
		private static Ship instance { get; set; }

		public static ShipPathSystem pathSystem => instance._pathSystem;

		[SerializeField] protected float          _minPitch = -5;
		[SerializeField] protected float          _maxPitch = 5;
		[SerializeField] protected float          _pitchSpeed;
		[SerializeField] protected float          _maxY    = -2;
		[SerializeField] protected float          _yChange = -.01f;
		[SerializeField] protected float          _y       = -2;
		[SerializeField] protected ShipPathSystem _pathSystem;

		private void Awake() {
			instance = this;
		}

		private void Update() {
			_y = Mathf.Min(_maxY, _y + _yChange * Time.deltaTime);
			transform.position = new Vector3(0, _y, 0);
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(_minPitch, _maxPitch, (Mathf.Sin(Time.time * _pitchSpeed) + 1) / 2));
		}
	}
}