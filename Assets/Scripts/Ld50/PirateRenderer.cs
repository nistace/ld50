using System.Linq;
using System.Text.RegularExpressions;
using Ld50;
using UnityEngine;
using UnityEngine.U2D;

public class PirateRenderer : MonoBehaviour {
	[SerializeField]                      protected SpriteRenderer               _renderer;
	[SerializeField]                      protected SpriteAtlas                  _atlas;
	[SerializeField]                      protected bool                         _lookLeft;
	[Header("Animation"), SerializeField] protected PirateAnimationSet           _animationSet;
	[SerializeField]                      protected float                        _animationSpeed = .1f;
	[SerializeField]                      protected int                          _pirateIndex;
	[SerializeField]                      protected int                          _animationsPerPirate = 9;
	[SerializeField]                      protected PirateAnimationSet.Animation _animation;

	public bool lookLeft {
		get => _lookLeft;
		set => _lookLeft = value;
	}

	public new PirateAnimationSet.Animation animation {
		get => _animation;
		set => _animation = value;
	}

	private Sprite[] _sprites { get; set; }

	private void Start() => LoadAtlas();

	[ContextMenu("Load Atlas")]
	private void LoadAtlas() {
		if (_atlas) {
			_sprites = new Sprite[_atlas.spriteCount];
			_atlas.GetSprites(_sprites);
			var dictionary = _sprites.ToDictionary(t => int.Parse(Regex.Match(t.name, ".*_(\\d+)").Groups[1].Value), t => t);
			_sprites = dictionary.OrderBy(t => t.Key).Select(t => t.Value).ToArray();
		}
	}

	private void Update() {
		_renderer.flipX = _lookLeft;
		_renderer.sprite = _sprites[_animationSet.GetCorrectedFrame(_animation, _animationsPerPirate * _pirateIndex, (int)(Time.time / _animationSpeed))];
	}
}