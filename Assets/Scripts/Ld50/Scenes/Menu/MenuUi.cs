using Ld50.Scenes.Menu;
using UnityEngine;

public class MenuUi : MonoBehaviour {
	[SerializeField] protected MainMenuUi  _main;
	[SerializeField] protected SettingsUi  _settings;
	[SerializeField] protected HowToPlayUi _howToPlay;

	public MainMenuUi  main      => _main;
	public SettingsUi  settings  => _settings;
	public HowToPlayUi howToPlay => _howToPlay;

	public enum Panel {
		Main,
		Settings,
		HowToPlay
	}

	private void Start() => ShowPanel(Panel.Main);

	public void ShowPanel(Panel panel) {
		_main.gameObject.SetActive(panel == Panel.Main);
		_settings.gameObject.SetActive(panel == Panel.Settings);
		_howToPlay.gameObject.SetActive(panel == Panel.HowToPlay);
	}

	public void ShowMain() => ShowPanel(Panel.Main);
	public void ShowSettings() => ShowPanel(Panel.Settings);
	public void ShowHowToPlay() => ShowPanel(Panel.HowToPlay);
}