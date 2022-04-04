using Ld50.Scenes.Menu;
using UnityEngine;

public class MenuUi : MonoBehaviour {
	[SerializeField] protected MainMenuUi        _main;
	[SerializeField] protected SettingsUi        _settings;
	[SerializeField] protected HowToPlayUi       _howToPlay;
	[SerializeField] protected YourPiratesNameUi _pirateName;
	[SerializeField] protected LeaderboardUi     _leaderboard;

	public MainMenuUi        main        => _main;
	public SettingsUi        settings    => _settings;
	public HowToPlayUi       howToPlay   => _howToPlay;
	public YourPiratesNameUi pirateName  => _pirateName;
	public LeaderboardUi     leaderboard => _leaderboard;

	private enum Panel {
		Main,
		Settings,
		HowToPlay,
		YourPirateName,
		Leaderboard
	}

	private void ShowPanel(Panel panel) {
		_main.gameObject.SetActive(panel == Panel.Main);
		_settings.gameObject.SetActive(panel == Panel.Settings);
		_howToPlay.gameObject.SetActive(panel == Panel.HowToPlay);
		_pirateName.gameObject.SetActive(panel == Panel.YourPirateName);
		_leaderboard.gameObject.SetActive(panel == Panel.Leaderboard);
	}

	public void ShowMain() => ShowPanel(Panel.Main);
	public void ShowSettings() => ShowPanel(Panel.Settings);
	public void ShowHowToPlay() => ShowPanel(Panel.HowToPlay);
	public void ShowYourPirateName() => ShowPanel(Panel.YourPirateName);
	public void ShowLeaderboard() => ShowPanel(Panel.Leaderboard);
}