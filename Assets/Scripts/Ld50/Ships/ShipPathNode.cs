using UnityEngine;

namespace Ld50.Ships {
	public class ShipPathNode : MonoBehaviour {
		[SerializeField] protected bool   _pointOfInterest;
		[SerializeField] protected string _overrideName;

		public bool    pointOfInterest => _pointOfInterest;
		public string  nodeName        => string.IsNullOrEmpty(_overrideName) ? name : _overrideName;
		public Vector2 position        => transform.position;
	}
}