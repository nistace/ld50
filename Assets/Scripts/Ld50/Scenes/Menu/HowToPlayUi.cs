using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace Ld50.Scenes.Menu {
	public class HowToPlayUi : MonoBehaviour {
		[SerializeField] protected GameObject[] _pages;
		[SerializeField] protected int          _pageIndex;
		[SerializeField] protected Button       _previousPageButton;
		[SerializeField] protected Button       _nextPageButton;
		[SerializeField] protected Button       _backButton;

		public UnityEvent onBackClicked => _backButton.onClick;

		private void Start() {
			ShowPage(0);
			_previousPageButton.onClick.AddListenerOnce(ShowPreviousPage);
			_nextPageButton.onClick.AddListenerOnce(ShowNextPage);
		}

		private void ShowPreviousPage() => ShowPage(_pageIndex - 1);
		private void ShowNextPage() => ShowPage(_pageIndex + 1);

		private void ShowPage(int page) {
			_pageIndex = Mathf.Clamp(page, 0, _pages.Length);
			for (var i = 0; i < _pages.Length; ++i) _pages[i].SetActive(i == _pageIndex);
			_previousPageButton.interactable = _pageIndex > 0;
			_nextPageButton.interactable = _pageIndex < _pages.Length - 1;
		}
	}
}