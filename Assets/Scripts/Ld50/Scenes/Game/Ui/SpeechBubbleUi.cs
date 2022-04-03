using Ld50.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ld50.Scenes.Game.Ui {
	public class SpeechBubbleUi : MonoBehaviour {
		[SerializeField] protected Image    _bubbleImage;
		[SerializeField] protected TMP_Text _text;
		[SerializeField] protected Image    _ideaImage;
		[SerializeField] protected float    _letterTime = 1;
		[SerializeField] protected float    _ideaTime   = 3;
		[SerializeField] protected FollowUi _follow;

		private float displayUntil { get; set; }
		private bool  idea         { get; set; }

		public Transform source {
			get => _follow.target;
			set => _follow.target = value;
		}

		public bool visible => Time.time < displayUntil;

		public void DisplayIdea(Sprite idea) {
			_ideaImage.sprite = idea;
			this.idea = true;
			displayUntil = Time.time + _ideaTime;
		}

		public void DisplayLetter(char lastLetter) {
			_text.text = $"{TextInputManager.lastLetter}";
			idea = false;
			displayUntil = Time.time + _letterTime;
		}

		private void Update() {
			_bubbleImage.enabled = Time.time < displayUntil;
			_text.enabled = Time.time < displayUntil && !idea;
			_ideaImage.enabled = Time.time < displayUntil && idea;
		}
	}
}