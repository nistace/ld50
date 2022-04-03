using UnityEngine;
using Utils.Extensions;

public class ShipCamera : MonoBehaviour {
	[SerializeField] protected Transform _target;
	[SerializeField] protected Vector3   _offset;
	[SerializeField] protected float     _minY;

	private void Update() {
		transform.position = _target.position + _offset;
		if (transform.position.y < _minY) transform.position = transform.position.With(y: _minY);
	}
}