using System;
using UnityEngine;

namespace Ld50.Ships {
	[Serializable]
	public class ShipPath {
		[SerializeField] protected ShipPathNode[]          _nodes;
		[SerializeField] protected ShipPathLink.LinkType[] _links;
		[SerializeField] protected int                     _nextNodeIndex;

		public bool                  destinationReached    => _nextNodeIndex >= _nodes.Length;
		public Vector2               nextNodeLocalPosition => _nodes[_nextNodeIndex].position;
		public ShipPathLink.LinkType nextNodeLinkType      => _links[_nextNodeIndex - 1];
		public ShipPathNode          currentNode           => _nodes.Length > 0 ? _nodes[Mathf.Min(_nextNodeIndex, _nodes.Length - 1)] : null;

		public ShipPath(ShipPathNode firstNode, ShipPathLink.LinkType firstNodeLinkType, ShipPath then) {
			_nodes = new ShipPathNode[then._nodes.Length + 1];
			_nodes[0] = firstNode;
			then._nodes.CopyTo(_nodes, 1);

			_links = new ShipPathLink.LinkType[then._links.Length + 1];
			_links[0] = firstNodeLinkType;
			then._links.CopyTo(_links, 1);

			_nextNodeIndex = 0;
		}

		public ShipPath(ShipPathNode node, ShipPathLink.LinkType link) : this(new[] { node }, new[] { link }) { }

		public ShipPath(ShipPathNode[] nodes, ShipPathLink.LinkType[] links) {
			_nodes = nodes;
			_links = links;
			_nextNodeIndex = 0;
		}

		public bool IsAtNextNode(Vector2 localPosition) => nextNodeLocalPosition == localPosition;
		public void ProgressToNextNode() => _nextNodeIndex++;
	}
}