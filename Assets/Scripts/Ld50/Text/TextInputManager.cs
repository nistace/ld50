using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ld50.Ships;
using UnityEngine;
using UnityEngine.Events;

namespace Ld50.Text {
	public class TextInputManager : MonoBehaviour {
		public static char lastLetter { get; private set; }
		public static bool listening  { get; set; }

		private static List<TextOption>                     options           { get; } = new List<TextOption>();
		public static  IEnumerable<ITextInteractableOption> allCurrentOptions => options;

		public static UnityEvent              onNewLetterEntered    { get; } = new UnityEvent();
		public static ITextInteractable.Event onInteractableSpelled { get; } = new ITextInteractable.Event();

		public static void SetOptions(IEnumerable<ITextInteractable> options) {
			TextInputManager.options.Clear();
			TextInputManager.options.AddRange(options.Select(t => new TextOption(t)));
		}

		public static ITextInteractableOption AddOption(ITextInteractable interactable) {
			var newOption = new TextOption(interactable);
			options.Add(newOption);
			return newOption;
		}

		private void Update() {
			if (!listening) return;
			if (!string.IsNullOrEmpty(Input.inputString) && Regex.IsMatch(Input.inputString, "^[a-zA-Z]$")) {
				lastLetter = Input.inputString.ToUpper()[0];
				ITextInteractable completedInteractable = null;
				foreach (var option in options) {
					if (option.nextChar == lastLetter) {
						option.SetProgress(option.progress + 1);
					}
					else if (option.progress > 0) {
						option.SetProgress(option.firstChar == lastLetter ? 1 : 0);
					}
					if (option.completed) {
						option.SetProgress(0);
						completedInteractable = option.interactable;
					}
				}
				onNewLetterEntered.Invoke();
				if (completedInteractable != null) onInteractableSpelled.Invoke(completedInteractable);
			}
		}

		public interface ITextInteractableOption {
			ITextInteractable interactable { get; }
			int               progress     { get; }
			bool              completed    { get; }

			UnityEvent onProgressChanged { get; }
		}

		private class TextOption : ITextInteractableOption {
			public  ITextInteractable interactable { get; }
			private string            upperKey     { get; }
			public  int               progress     { get; private set; }
			public  int               firstChar    => upperKey[0];
			public  char              nextChar     => upperKey[progress];
			public  bool              completed    => progress == upperKey.Length;

			public UnityEvent onProgressChanged { get; } = new UnityEvent();

			public TextOption(ITextInteractable interactable) {
				this.interactable = interactable;
				upperKey = interactable.interactableText.ToUpper();
				SetProgress(0);
			}

			public void SetProgress(int progress) {
				if (this.progress == progress) return;
				this.progress = progress;
				while (!completed && (nextChar < 'A' || nextChar > 'Z')) this.progress++;
				onProgressChanged.Invoke();
			}
		}
	}
}