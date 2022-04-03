using System;
using UnityEngine;

namespace Ld50 {
	[Serializable]
	public class PirateAnimationSet {
		[SerializeField] protected Vector2Int[] _animationFrames;
		[SerializeField] protected float[]      _animationSpeeds;

		public enum Animation {
			Idle          = 0,
			Walk          = 1,
			Climb         = 2,
			LookOut       = 3,
			PlayAccordion = 4,
			Till          = 5,
			FishPlanks    = 6,
			Repair
		}

		public int GetCorrectedFrame(Animation animation, int offset) {
			var firstFrame = _animationFrames[(int)animation].x;
			var lastFrame = _animationFrames[(int)animation].y;
			var countFrames = lastFrame - firstFrame + 1;
			return offset + firstFrame + (int)(Time.time * _animationSpeeds[(int)animation]) % countFrames;
		}
	}
}