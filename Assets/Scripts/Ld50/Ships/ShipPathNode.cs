using UnityEngine;

namespace Ld50.Ships {
	public class ShipPathNode : MonoBehaviour {
		[SerializeField] protected bool _lastNode;

		public Vector2 position      => transform.position;
		public Vector2 localPosition => transform.localPosition;
		public bool    lastNode      => _lastNode;
	}
}