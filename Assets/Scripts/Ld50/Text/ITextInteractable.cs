using UnityEngine;
using UnityEngine.Events;

namespace Ld50.Text {
	public interface ITextInteractable {
		public class Event : UnityEvent<ITextInteractable> { }

		string    interactableText       { get; }
		Sprite    interactableIdeaSprite { get; }
		Transform transform              { get; }
	}
}