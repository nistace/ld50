using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils.Extensions;

namespace Ld50.Ships {
	public class ShipHull : MonoBehaviour {
		[SerializeField] protected Tilemap    _hullTileMap;
		[SerializeField] protected RectInt    _bounds;
		[SerializeField] protected TileBase[] _undamagedHullTiles;
		[SerializeField] protected TileBase[] _damagedHullTiles;
		[SerializeField] protected TileBase[] _fixedHullTiles;
		[SerializeField] protected Vector2    _coordinatesToPosition;

		private HashSet<Vector3Int> damageableHullPositions { get; } = new HashSet<Vector3Int>();

		private Dictionary<Vector3Int, ShipNodeTask> holesAndTasks { get; } = new Dictionary<Vector3Int, ShipNodeTask>();

		private void Start() {
			for (var x = _bounds.xMin; x <= _bounds.xMax; ++x)
			for (var y = _bounds.yMin; y <= _bounds.yMax; ++y) {
				var coordinates = new Vector3Int(x, y, 0);
				if (_hullTileMap.GetTile(coordinates).In(_undamagedHullTiles)) {
					damageableHullPositions.Add(coordinates);
				}
			}
		}

		public bool TryGetRandomNewHolePosition(out Vector3Int coordinates, out Vector2 position) {
			coordinates = damageableHullPositions.Random();
			position = coordinates + (Vector3)_coordinatesToPosition;
			if (coordinates.y % 2 == 1) position += Vector2.down;
			return true;
		}

		public void AddHole(Vector3Int coordinates, ShipNodeTask task) {
			if (!damageableHullPositions.Contains(coordinates)) throw new ArgumentException($"A hole cannot be created at {coordinates}");
			damageableHullPositions.Remove(coordinates);
			holesAndTasks.Add(coordinates, task);
			_hullTileMap.SetTile(coordinates, _damagedHullTiles.Random());
		}

		public void FixHole(Vector3Int coordinates) {
			if (!holesAndTasks.ContainsKey(coordinates)) throw new ArgumentException($"A hole cannot be fixed at {coordinates}");
			_hullTileMap.SetTile(coordinates, _fixedHullTiles[_damagedHullTiles.IndexOf(_hullTileMap.GetTile(coordinates))]);
			damageableHullPositions.Add(coordinates);
			holesAndTasks.Remove(coordinates);
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