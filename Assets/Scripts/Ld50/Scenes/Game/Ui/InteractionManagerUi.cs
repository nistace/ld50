using System.Collections.Generic;
using System.Linq;
using Ld50.Text;
using UnityEngine;

namespace Ld50.Scenes.Game.Ui {
	public class InteractionManagerUi : MonoBehaviour {
		private static InteractionManagerUi instance { get; set; }

		[SerializeField] protected InteractableItemUi _overItemPrefab;
		[SerializeField] protected InteractableItemUi _underItemPrefab;
		[SerializeField] protected FollowUi           _marker;

		private static HashSet<InteractableItemUi> activeItems { get; } = new HashSet<InteractableItemUi>();
		private static Queue<InteractableItemUi>   overPool    { get; } = new Queue<InteractableItemUi>();
		private static Queue<InteractableItemUi>   underPool   { get; } = new Queue<InteractableItemUi>();

		private void Awake() {
			instance = this;
			activeItems.Clear();
			overPool.Clear();
			underPool.Clear();
		}

		public static void HideAll() {
			foreach (var activeItem in activeItems) {
				if (activeItem.over) overPool.Enqueue(activeItem);
				else underPool.Enqueue(activeItem);
				activeItem.gameObject.SetActive(false);
			}
			activeItems.Clear();
		}

		public static void Show(TextInputManager.ITextInteractableOption option, bool over = true) {
			var itemUi = GetItemUi(over);
			itemUi.gameObject.SetActive(true);
			itemUi.SetInteractable(option);
			activeItems.Add(itemUi);
		}

		public static void ShowAll(IEnumerable<TextInputManager.ITextInteractableOption> options) {
			var orderedOptions = options.OrderBy(t => t.interactable.transform.localPosition.y).ThenBy(t => t.interactable.transform.localPosition.x).Reverse();
			var previousOptionTargetPosition = new Vector2(-50, -50);
			var previousWasOver = false;
			foreach (var option in orderedOptions) {
				var optionPosition = option.interactable.transform.localPosition;
				var over = Mathf.Abs(optionPosition.y - previousOptionTargetPosition.y) > 1 || Mathf.Abs(optionPosition.x - previousOptionTargetPosition.x) > 5 || !previousWasOver;
				Show(option, over);
				previousOptionTargetPosition = optionPosition;
				previousWasOver = over;
			}
		}

		private static InteractableItemUi GetItemUi(bool over) {
			if (over) {
				if (overPool.Count > 0) return overPool.Dequeue();
				return Instantiate(instance._overItemPrefab, instance.transform);
			}
			if (underPool.Count > 0) return underPool.Dequeue();
			return Instantiate(instance._underItemPrefab, instance.transform);
		}

		public static void ShowMarker(Transform target) {
			instance._marker.target = target;
			instance._marker.gameObject.SetActive(true);
		}

		public static void HideMarker() => instance._marker.gameObject.SetActive(false);
	}
}