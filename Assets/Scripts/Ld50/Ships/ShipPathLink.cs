using System;
using UnityEngine;

namespace Ld50.Ships {
	[Serializable]
	public class ShipPathLink {
		[SerializeField] protected ShipPathNode _firstNode;
		[SerializeField] protected ShipPathNode _secondNode;
		[SerializeField] protected LinkType     _type;

		public ShipPathNode firstNode  => _firstNode;
		public ShipPathNode secondNode => _secondNode;
		public LinkType     type       => _type;

		public enum LinkType {
			Walk  = 0,
			Climb = 1,
		}

		public bool Contains(ShipPathNode node) => _firstNode == node || _secondNode == node;
	}
}