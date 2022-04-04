using TMPro;
using UnityEngine;

namespace Ld50.Scenes.Menu {
	public class LeaderboardEntryUi : MonoBehaviour {
		[SerializeField] protected TMP_Text _name;
		[SerializeField] protected TMP_Text _score;

		public void Set(string name, int score) {
			_name.text = name;
			_score.text = $"{score}";
		}
	}
}