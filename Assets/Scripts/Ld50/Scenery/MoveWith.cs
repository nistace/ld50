using UnityEngine;

public class MoveWith : MonoBehaviour {
	[SerializeField] protected Transform _with;
	[SerializeField] protected float     _withMinY;
	[SerializeField] protected float     _withMaxY;
	[SerializeField] protected float     _thisMinY;
	[SerializeField] protected float     _thisMaxY;

	private void Update() {
		if (!_with) return;
		transform.position = new Vector3(0, Mathf.Lerp(_thisMinY, _thisMaxY, (_with.transform.position.y - _withMinY) / (_withMaxY - _withMinY)), 0);
	}
}