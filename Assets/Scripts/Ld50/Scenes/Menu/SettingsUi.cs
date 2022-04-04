using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Audio;
using Utils.Extensions;

namespace Ld50.Scenes.Menu {
	public class SettingsUi : MonoBehaviour {
		[SerializeField] protected Slider _musicSlider;
		[SerializeField] protected Slider _voiceSlider;
		[SerializeField] protected Slider _sfxSlider;
		[SerializeField] protected Button _backButton;

		public UnityEvent onBackClicked => _backButton.onClick;

		private void Start() {
			_musicSlider.onValueChanged.AddListenerOnce(t => AudioManager.Music.volume = t);
			_voiceSlider.onValueChanged.AddListenerOnce(SetVoiceVolume);
			_sfxSlider.onValueChanged.AddListenerOnce(SetSfxVolume);
		}

		private static void SetVoiceVolume(float volume) {
			AudioManager.Voices.volume = volume;
			AudioManager.Voices.PlayRandom("captain.noise");
		}

		private static void SetSfxVolume(float volume) {
			AudioManager.Sfx.volume = volume;
			AudioManager.Sfx.PlayRandom("repair");
		}
	}
}