using Ld50.Text;
using TMPro;
using UnityEngine;
using Utils.Extensions;

namespace Ld50.Scenes.Game.Ui {
	public class InteractableItemUi : MonoBehaviour {
		[SerializeField] protected FollowUi _follow;
		[SerializeField] protected TMP_Text _text;
		[SerializeField] protected Color    _highlightColor = new Color(1, .5f, 0);
		[SerializeField] protected bool     _over;

		public  bool                                     over   => _over;
		private TextInputManager.ITextInteractableOption option { get; set; }

		public void SetInteractable(TextInputManager.ITextInteractableOption option) {
			this.option = option;
			_follow.target = option.interactable.transform;
			option.onProgressChanged.AddListenerOnce(Refresh);
			Refresh();
		}

		private void Refresh() {
			if (option.progress == 0) _text.text = option.interactable.interactableText;
			var text = option.interactable.interactableText;
			_text.text = $"<{_highlightColor.ToHexaString(true)}>{text.Substring(0, option.progress)}</color>{text.Substring(option.progress)}";
		}
	}
}