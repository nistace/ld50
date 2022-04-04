using System.Collections;
using Ld50.Scenes.CommonUi;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

public class MenuManager : MonoBehaviour {
	[SerializeField] protected MenuUi _ui;

	private void Start() {
		StartCoroutine(TransitionScreenUi.Fade(0, 1));
		_ui.main.onNewGameClicked.AddListenerOnce(HandleNewGame);
		_ui.main.onSettingsClicked.AddListenerOnce(_ui.ShowSettings);
		_ui.main.onHowToPlayClicked.AddListenerOnce(_ui.ShowHowToPlay);
		_ui.settings.onBackClicked.AddListenerOnce(_ui.ShowMain);
		_ui.howToPlay.onBackClicked.AddListenerOnce(_ui.ShowMain);
	}

	private void HandleNewGame() => StartCoroutine(StartNewGame());

	private IEnumerator StartNewGame() {
		yield return StartCoroutine(TransitionScreenUi.Fade(1, 1));
		SceneManager.LoadScene("Game");
	}
}