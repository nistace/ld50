using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;
using Web;

namespace Ld50.Scenes.Menu {
	public class LeaderboardUi : MonoBehaviour {
		[SerializeField] protected GameObject         _loadingMessage;
		[SerializeField] protected GameObject         _boardGameObject;
		[SerializeField] protected LeaderboardEntryUi _entryPrefab;
		[SerializeField] protected Transform          _container;
		[SerializeField] protected Button             _backButton;

		public UnityEvent onBackClicked => _backButton.onClick;

		public void Populate(Leaderboard leaderboard) {
			_loadingMessage.SetActive(false);
			_boardGameObject.SetActive(true);

			foreach (var entry in leaderboard.result) {
				Instantiate(_entryPrefab, _container).Set(entry.name, entry.score);
			}
		}
	}
}