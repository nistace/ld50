using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ld50.Scenes.Menu {
	public class MainMenuUi : MonoBehaviour {
		[SerializeField] protected Button _howToPlayButton;
		[SerializeField] protected Button _newGameButton;
		[SerializeField] protected Button _settingsButton;
		[SerializeField] protected Button _leaderboardButton;

		public UnityEvent onHowToPlayClicked   => _howToPlayButton.onClick;
		public UnityEvent onNewGameClicked     => _newGameButton.onClick;
		public UnityEvent onSettingsClicked    => _settingsButton.onClick;
		public UnityEvent onLeaderboardClicked => _leaderboardButton.onClick;
	}
}