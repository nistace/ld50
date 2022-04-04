using System;
using Ld50.Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Utils.Ui;

public class StatsUi : PanelUi {
	[SerializeField] protected Image    _moraleBarFill;
	[SerializeField] protected Image    _shipSpeedBarFill;
	[SerializeField] protected TMP_Text _planksText;
	[SerializeField] protected TMP_Text _enemyShipDelayText;
	[SerializeField] protected Image    _waterBarFill;
	[SerializeField] protected Image[]  _waterFillSpeedImages;
	[SerializeField] protected float    _waterFillSpeedImageOffset = .2f;
	[SerializeField] protected TMP_Text _destinationText;

	private void Start() {
		Open();
	}

	private void Update() {
		_moraleBarFill.fillAmount = Ship.taskManager.morale.Remap(Ship.taskManager.minMorale, Ship.taskManager.maxMorale, 0, 1);
		_shipSpeedBarFill.fillAmount = Ship.taskManager.shipSpeed.Remap(Ship.taskManager.minShipSpeed, Ship.taskManager.maxShipSpeed, 0, 1);
		_planksText.text = $"{Ship.taskManager.plankCount}";
		_enemyShipDelayText.text = Ship.taskManager.shipDetectionEnabled ? $"{Ship.taskManager.enemyShipRealDelay:0.0}" : "?";
		_waterBarFill.fillAmount = Ship.y.Remap(Ship.gameOverY, Ship.maxY, 1, 0);

		for (var i = 0; i < _waterFillSpeedImages.Length; ++i) {
			var speedRequirement = _waterFillSpeedImageOffset * (i - _waterFillSpeedImages.Length / 2f);
			_waterFillSpeedImages[i].enabled = speedRequirement > 0 && Ship.taskManager.sinkingSpeed >= speedRequirement || speedRequirement < 0 && Ship.taskManager.sinkingSpeed <= speedRequirement;
		}
		_destinationText.text = $@"Destination {Ship.taskManager.distanceToDestination / 1000:0000.00}km";
	}
}