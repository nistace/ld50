using System.Collections;
using Ld50.Scenes.CommonUi;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;
using Web;

public class MenuManager : MonoBehaviour {
	[SerializeField] protected MenuUi _ui;

	private void Start() {
		if (string.IsNullOrEmpty(Pirate.captainsName)) _ui.ShowYourPirateName();
		else _ui.ShowMain();
		StartCoroutine(TransitionScreenUi.Fade(0, 1));
		StartCoroutine(WebManager.GetLeaderBoard(_ui.leaderboard.Populate));
		_ui.main.onNewGameClicked.AddListenerOnce(HandleNewGame);
		_ui.main.onSettingsClicked.AddListenerOnce(_ui.ShowSettings);
		_ui.main.onHowToPlayClicked.AddListenerOnce(_ui.ShowHowToPlay);
		_ui.main.onLeaderboardClicked.AddListenerOnce(_ui.ShowLeaderboard);
		_ui.settings.onBackClicked.AddListenerOnce(_ui.ShowMain);
		_ui.howToPlay.onBackClicked.AddListenerOnce(_ui.ShowMain);
		_ui.pirateName.onValidateClicked.AddListenerOnce(HandlePirateNameValidated);
		_ui.leaderboard.onBackClicked.AddListenerOnce(_ui.ShowMain);
	}

	private void HandlePirateNameValidated() {
		Pirate.captainsName = _ui.pirateName.nameValue;
		_ui.ShowMain();
	}

	private void HandleNewGame() => StartCoroutine(StartNewGame());

	private IEnumerator StartNewGame() {
		yield return StartCoroutine(TransitionScreenUi.Fade(1, 1));
		SceneManager.LoadScene("Game");
	}
}