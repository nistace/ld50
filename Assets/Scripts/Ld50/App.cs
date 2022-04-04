using System;
using System.Linq;
using UnityEngine;
using Utils.Libraries;

public class App : MonoBehaviour {
	private static App instance { get; set; }

	private void Awake() {
		if (instance) Destroy(gameObject);
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start() {
		AudioClips.LoadLibrary(Resources.LoadAll<AudioClipLibrary>("Libraries").FirstOrDefault());
		Sprites.LoadLibrary(Resources.LoadAll<SpriteLibrary>("Libraries").FirstOrDefault());
	}
}