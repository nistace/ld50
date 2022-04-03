using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ld50.Ships {
	public class ShipPathSystem : MonoBehaviour {
		[SerializeField] protected ShipPathLink[]     _links;
		[SerializeField] protected List<ShipPathNode> _temporaryNodes = new List<ShipPathNode>();
		[SerializeField] protected List<ShipPathLink> _temporaryLinks = new List<ShipPathLink>();

		private List<ShipPathNode> nodes { get; } = new List<ShipPathNode>();

		public void Initialize() {
			nodes.Clear();
			nodes.AddRange(_links.SelectMany(t => new[] { t.firstNode, t.secondNode }).Distinct());
		}

		public void AddTemporaryNode(ShipPathNode node) {
			_temporaryNodes.Add(node);
			var otherNodesOnSameLevel = nodes.Where(t => Mathf.Abs(t.localPosition.y - node.localPosition.y) < .1f).ToArray();
			if (otherNodesOnSameLevel.Length == 0) throw new ArgumentException($"No neighbour node at y={node.localPosition.y}");
			ShipPathNode leftNeighbour = null;
			ShipPathNode rightNeighbour = null;
			foreach (var t in otherNodesOnSameLevel) {
				if (t.localPosition.x < node.localPosition.x) {
					if (leftNeighbour == null || leftNeighbour.localPosition.x < t.localPosition.x) leftNeighbour = t;
				}
				else {
					if (rightNeighbour == null || rightNeighbour.localPosition.x > t.localPosition.x) rightNeighbour = t;
				}
			}

			if (rightNeighbour != null) {
				_temporaryLinks.Add(new ShipPathLink(rightNeighbour, node, ShipPathLink.LinkType.Walk));
				if (rightNeighbour.lastNode && !leftNeighbour) node.transform.localPosition = rightNeighbour.localPosition;
			}
			if (leftNeighbour != null) {
				_temporaryLinks.Add(new ShipPathLink(leftNeighbour, node, ShipPathLink.LinkType.Walk));
				if (leftNeighbour.lastNode && !rightNeighbour) node.transform.localPosition = leftNeighbour.localPosition;
			}
		}

		public bool TryGetPath(ShipPathNode from, ShipPathNode to, out ShipPath path, ShipPathLink origin = null) {
			if (from == to) {
				path = new ShipPath(from, origin?.type ?? ShipPathLink.LinkType.Walk);
				return true;
			}

			if (_temporaryNodes.Contains(from) && TryFindPathStartingWith(_temporaryLinks.Where(t => t.Contains(from)), from, origin, to, out path)) return true;
			if (_temporaryNodes.Contains(to) && TryFindPathStartingWith(_temporaryLinks.Where(t => t.Contains(to)), from, origin, to, out path)) return true;
			if (TryFindPathStartingWith(_links, from, origin, to, out path)) return true;
			return false;
		}

		private bool TryFindPathStartingWith(IEnumerable<ShipPathLink> links, ShipPathNode from, ShipPathLink origin, ShipPathNode to, out ShipPath path) {
			foreach (var link in links) {
				if (!link.Contains(from)) continue;
				if (origin != null && origin.Contains(link.Other(from))) continue;
				if (!TryGetPath(link.Other(from), to, out var subPath, link)) continue;
				path = new ShipPath(from, link.type, subPath);
				return true;
			}
			path = null;
			return false;
		}

		public void RemoveTemporaryNode(ShipPathNode node) {
			_temporaryNodes.Remove(node);
			_temporaryLinks.RemoveAll(t => t.Contains(node));
		}

		public ShipPathNode GetClosestNode(Vector3 localPosition) {
			var otherNodesOnSameLevel = nodes.Where(t => Mathf.Abs(t.localPosition.y - localPosition.y) < .1f).ToArray();
			if (otherNodesOnSameLevel.Length == 0) throw new ArgumentException($"No neighbour node at y={localPosition.y}");
			ShipPathNode closest = null;
			foreach (var t in otherNodesOnSameLevel) {
				if (closest == null || Mathf.Abs(closest.localPosition.x - localPosition.x) > Mathf.Abs((t.localPosition.x - localPosition.x))) {
					closest = t;
				}
			}
			return closest;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.color = Color.cyan;

			foreach (var link in _links.Union(_temporaryLinks)) {
				Gizmos.color = Selection.activeGameObject == link.firstNode.gameObject || Selection.activeGameObject == link.secondNode.gameObject ? Color.magenta : Color.cyan;
				Gizmos.DrawLine(link.firstNode.position, link.secondNode.position);
			}

			var displayNodes = this.nodes.Count > 0 ? this.nodes : _links.SelectMany(t => new[] { t.firstNode, t.secondNode }).Distinct();
			foreach (var node in displayNodes.Union(_temporaryNodes)) {
				Gizmos.color = Selection.activeGameObject == node.gameObject ? Color.magenta : Color.cyan;
				var textStyle = new GUIStyle { normal = { textColor = Selection.activeGameObject == node.gameObject ? Color.magenta : Color.cyan } };
				Gizmos.DrawSphere(node.position, .1f);
				Handles.Label(node.position + new Vector2(0, .3f), node.name, textStyle);
			}
		}
#endif
	}
}