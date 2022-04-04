using UnityEngine;

namespace Ld50.Scenes.Game.Ui {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected GameOverUi _gameOver;
		[SerializeField] protected StatsUi    _stats;

		public StatsUi    stats    => _stats;
		public GameOverUi gameOver => _gameOver;

		private void Start() {
			gameOver.gameObject.SetActive(false);
		}
	}
}