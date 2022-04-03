using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;
using Random = UnityEngine.Random;

namespace Ld50.Ships {
	public class ShipHull : MonoBehaviour {
		[SerializeField] protected Tilemap    _hullTileMap;
		[SerializeField] protected RectInt    _bounds;
		[SerializeField] protected TileBase[] _undamagedHullTiles;
		[SerializeField] protected TileBase[] _damagedHullTiles;
		[SerializeField] protected TileBase[] _fixedHullTiles;
		[SerializeField] protected Vector2    _coordinatesToPosition;

		private HashSet<Vector3Int> damageableHullPositions { get; } = new HashSet<Vector3Int>();

		private void Start() {
			for (var x = _bounds.xMin; x <= _bounds.xMax; ++x)
			for (var y = _bounds.yMin; y <= _bounds.yMax; ++y) {
				var coordinates = new Vector3Int(x, y, 0);
				if (_hullTileMap.GetTile(coordinates).In(_undamagedHullTiles)) {
					damageableHullPositions.Add(coordinates);
				}
			}
		}

		public bool TryAddHole(out Vector2 position) {
			var coordinates = damageableHullPositions.Random();
			_hullTileMap.SetTile(coordinates, _damagedHullTiles.Random());
			position = transform.position + coordinates + (Vector3)_coordinatesToPosition;
			return true;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			Gizmos.color = Color.green;

			Gizmos.DrawLine(transform.position + new Vector3(_bounds.xMin, _bounds.yMin, 0), transform.position + new Vector3(_bounds.xMin, _bounds.yMax, 0));
			Gizmos.DrawLine(transform.position + new Vector3(_bounds.xMin, _bounds.yMin, 0), transform.position + new Vector3(_bounds.xMax, _bounds.yMin, 0));
			Gizmos.DrawLine(transform.position + new Vector3(_bounds.xMax, _bounds.yMax, 0), transform.position + new Vector3(_bounds.xMax, _bounds.yMin, 0));
			Gizmos.DrawLine(transform.position + new Vector3(_bounds.xMax, _bounds.yMax, 0), transform.position + new Vector3(_bounds.xMin, _bounds.yMax, 0));
		}
#endif
	}
}