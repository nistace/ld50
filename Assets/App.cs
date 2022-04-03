using System.Linq;
using UnityEngine;
using Utils.Libraries;

public class App : MonoBehaviour {
	private void Start() {
		AudioClips.LoadLibrary(Resources.LoadAll<AudioClipLibrary>("Libraries").FirstOrDefault());
	}
}