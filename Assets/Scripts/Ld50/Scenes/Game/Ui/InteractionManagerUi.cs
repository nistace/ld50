using System.Collections.Generic;
using Ld50.Text;
using UnityEngine;

namespace Ld50.Scenes.Game.Ui {
	public class InteractionManagerUi : MonoBehaviour {
		private static InteractionManagerUi instance { get; set; }

		[SerializeField] protected InteractableItemUi _itemPrefab;
		[SerializeField] protected FollowUi           _marker;

		private static HashSet<InteractableItemUi> activeItems { get; } = new HashSet<InteractableItemUi>();
		private static Queue<InteractableItemUi>   pool        { get; } = new Queue<InteractableItemUi>();

		private void Awake() {
			instance = this;
		}

		public static void HideAll() {
			foreach (var activeItem in activeItems) {
				pool.Enqueue(activeItem);
				activeItem.gameObject.SetActive(false);
			}
			activeItems.Clear();
		}

		public static void ShowAll(IEnumerable<TextInputManager.ITextInteractableOption> options) {
			foreach (var option in options) {
				var itemUi = pool.Count > 0 ? pool.Dequeue() : Instantiate(instance._itemPrefab, instance.transform);
				itemUi.gameObject.SetActive(true);
				itemUi.SetInteractable(option);
				activeItems.Add(itemUi);
			}
		}

		public static void ShowMarker(Transform target) {
			instance._marker.target = target;
			instance._marker.gameObject.SetActive(true);
		}

		public static void HideMarker() => instance._marker.gameObject.SetActive(false);
	}
}