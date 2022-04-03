using UnityEngine;

namespace Ld50.Ships {
	public class ShipFishingRod : MonoBehaviour {
		[SerializeField] protected SpriteRenderer _renderer;
		[SerializeField] protected Transform      _tip;

		public bool      inUse { get; private set; }
		public Transform tip   => _tip;

		public void SetInUse(bool inUse) {
			_renderer.enabled = inUse;
			this.inUse = inUse;
		}
	}
}