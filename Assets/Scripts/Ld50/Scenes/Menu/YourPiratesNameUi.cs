using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace Ld50.Scenes.Menu {
	public class YourPiratesNameUi : MonoBehaviour {
		[SerializeField] protected TMP_InputField _nameInput;
		[SerializeField] protected Button         _validateButton;

		public string nameValue => _nameInput.text;

		public UnityEvent onValidateClicked => _validateButton.onClick;

		private void Start() => _nameInput.onValueChanged.AddListenerOnce(HandleValueChanged);

		private void HandleValueChanged(string name) => _validateButton.interactable = name.Length > 0;
	}
}