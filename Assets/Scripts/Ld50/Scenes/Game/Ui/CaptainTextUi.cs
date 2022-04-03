using Ld50.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Ld50.Scenes.Game.Ui {
	public class CaptainTextUi : MonoBehaviour {
		[SerializeField] protected Image    _bubbleImage;
		[SerializeField] protected TMP_Text _text;
		[SerializeField] protected Image    _ideaImage;
		[SerializeField] protected float    _timeBeforeHide = 1;
		[SerializeField] protected float    _ideaTime       = 3;

		private float hideTime         { get; set; }
		private float displayIdeaUntil { get; set; }

		private void Start() {
			TextInputManager.onNewLetterEntered.AddListenerOnce(HandleNewLetterEntered);
			TextInputManager.onInteractableSpelled.AddListenerOnce(DisplayIdea);
		}

		private void DisplayIdea(ITextInteractable idea) {
			_ideaImage.sprite = idea.interactableIdeaSprite;
			displayIdeaUntil = Time.time + _ideaTime;
		}

		private void HandleNewLetterEntered() {
			_text.text = $"{TextInputManager.lastLetter}";
			hideTime = Time.time + _timeBeforeHide;
		}

		private void Update() {
			_bubbleImage.enabled = Time.time < displayIdeaUntil || Time.time < hideTime;
			_text.enabled = Time.time < hideTime && Time.time > displayIdeaUntil;
			_ideaImage.enabled = Time.time < displayIdeaUntil;
		}
	}
}