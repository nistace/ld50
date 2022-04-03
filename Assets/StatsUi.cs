using Ld50.Ships;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class StatsUi : MonoBehaviour {
	[SerializeField] protected Image    _moraleBarFill;
	[SerializeField] protected Image    _shipSpeedBarFill;
	[SerializeField] protected TMP_Text _planksText;
	[SerializeField] protected TMP_Text _enemyShipDelayText;

	private void Update() {
		_moraleBarFill.fillAmount = Ship.taskManager.morale.Remap(Ship.taskManager.minMorale, Ship.taskManager.maxMorale, 0, 1);
		_shipSpeedBarFill.fillAmount = Ship.taskManager.shipSpeed.Remap(Ship.taskManager.minShipSpeed, Ship.taskManager.maxShipSpeed, 0, 1);
		_planksText.text = $"{Ship.taskManager.plankCount}";
		_enemyShipDelayText.text = Ship.taskManager.shipDetectionEnabled ? $"{Ship.taskManager.enemyShipRealDelay:0.0}" : "?";
	}
}