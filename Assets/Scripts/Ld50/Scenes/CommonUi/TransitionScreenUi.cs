using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

namespace Ld50.Scenes.CommonUi {
	public class TransitionScreenUi : MonoBehaviour {
		private static TransitionScreenUi instance { get; set; }

		[SerializeField] private   Image _fullScreenImage;
		[SerializeField] protected bool  _opaqueOnAwake;

		private void Awake() {
			if (!instance) {
				instance = this;
				_fullScreenImage.color = _fullScreenImage.color.With(a: _opaqueOnAwake ? 1 : 0);
			}
		}

		public static IEnumerator Fade(float targetOpacity, float time) {
			var startTime = Time.time;
			var startA = instance._fullScreenImage.color.a;
			while (Time.time < startTime + time) {
				instance._fullScreenImage.color = instance._fullScreenImage.color.With(a: Mathf.Lerp(startA, targetOpacity, (Time.time - startTime) / time));
				yield return null;
			}
			instance._fullScreenImage.color = instance._fullScreenImage.color.With(a: targetOpacity);
			instance._fullScreenImage.raycastTarget = targetOpacity > 0;
		}
	}
}