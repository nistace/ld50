using System.Collections.Generic;
using Ld50.Text;
using UnityEngine;
using Utils.Extensions;

namespace Ld50.Scenes.Game.Ui {
	public class SpeechBubbleManagerUi : MonoBehaviour {
		private static SpeechBubbleManagerUi instance { get; set; }

		[SerializeField] protected SpeechBubbleUi _captainBubble;
		[SerializeField] protected Pirate         _captain;
		[SerializeField] protected SpeechBubbleUi _bubblePrefab;

		private static List<SpeechBubbleUi> otherBubbles { get; } = new List<SpeechBubbleUi>();

		private void Awake() {
			instance = this;
		}

		private void Start() {
			TextInputManager.onNewLetterEntered.AddListenerOnce(HandleNewLetterEntered);
			TextInputManager.onInteractableSpelled.AddListenerOnce(HandleInteractableSpelled);
		}

		private void HandleInteractableSpelled(ITextInteractable idea) => _captainBubble.DisplayIdea(idea.interactableIdeaSprite);
		private void HandleNewLetterEntered() => _captainBubble.DisplayLetter(TextInputManager.lastLetter);

		public static void ShowIdea(Transform source, Sprite idea) {
			if (!otherBubbles.TryFirst(t => t.source == source, out var bubble) && !otherBubbles.TryFirst(t => !t.visible, out bubble)) {
				bubble = Instantiate(instance._bubblePrefab, instance.transform);
				otherBubbles.Add(bubble);
			}
			bubble.source = source;
			bubble.DisplayIdea(idea);
		}
	}
}