using System;

namespace Web {
	[Serializable]
	public class Leaderboard {
		public LeaderboardEntry[] result;

		[Serializable]
		public class LeaderboardEntry {
			public int    id;
			public string name;
			public int    score;
			public string time;
		}
	}
}