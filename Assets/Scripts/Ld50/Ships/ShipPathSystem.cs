using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ld50.Ships {
	public class ShipPathSystem : MonoBehaviour {
		[SerializeField] protected ShipPathLink[] _links;

		public bool TryGetPath(ShipPathNode from, ShipPathNode to, out ShipPath path, ShipPathLink origin = null) {
			if (from == to) {
				path = new ShipPath(from, origin?.type ?? ShipPathLink.LinkType.Walk);
				return true;
			}

			foreach (var link in _links) {
				if (link.firstNode == from && (origin == null || !origin.Contains(link.secondNode))) {
					if (TryGetPath(link.secondNode, to, out var subPath, link)) {
						path = new ShipPath(from, link.type, subPath);
						return true;
					}
				}
				if (link.secondNode == from && (origin == null || !origin.Contains(link.firstNode))) {
					if (TryGetPath(link.firstNode, to, out var subPath, link)) {
						path = new ShipPath(from, link.type, subPath);
						return true;
					}
				}
			}
			path = null;
			return false;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.color = Color.cyan;

			foreach (var link in _links) {
				Gizmos.DrawLine(link.firstNode.position, link.secondNode.position);
			}

			foreach (var node in _links.SelectMany(t => new[] { t.firstNode, t.secondNode }).Distinct()) {
				Gizmos.color = Selection.activeGameObject == node.gameObject ? Color.magenta : Color.cyan;
				var textStyle = new GUIStyle { normal = { textColor = Selection.activeGameObject == node.gameObject ? Color.magenta : Color.cyan } };
				Gizmos.DrawSphere(node.position, .1f);
				Handles.Label(node.position + new Vector2(0, .3f), node.pointOfInterest ? node.nodeName.ToUpper() : node.nodeName.ToLower(), textStyle);
			}
		}
#endif
	}
}