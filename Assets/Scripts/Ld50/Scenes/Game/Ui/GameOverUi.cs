using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverUi : MonoBehaviour {
	[SerializeField] protected TMP_Text _timeText;
	[SerializeField] protected TMP_Text _distanceText;
	[SerializeField] protected TMP_Text _rankingText;
	[SerializeField] protected Button   _continueButton;

	public UnityEvent onContinueClicked => _continueButton.onClick;

	public void Show(int time, string formattedDistanceLeft, int ranking, string rankingInfo) {
		_timeText.text = "You have been very rich during <b>" + time + "</b> seconds.";

		_distanceText.text = $"You only had to travel <b>{formattedDistanceLeft}</b> more meters to get home.";

		_rankingText.text = ranking switch {
			0 => "Nobody ever got that close! Congratulations!",
			1 => $"Only <b>1</b> captain went farther: {rankingInfo}.",
			_ => $"{rankingInfo}, and <b>{ranking - 1}</b> others also went farther than you."
		};
		gameObject.SetActive(true);
	}
}