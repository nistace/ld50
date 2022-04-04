using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Web {
	public static class WebManager {
		private static string apiUrlRoot => "https://nathanistace.be/gamejams/ludumdare50/api/";

		public static IEnumerator GetLeaderBoard(Action<Leaderboard> callback) {
			using (var webRequest = new UnityWebRequest($"{apiUrlRoot}leaderboard", "GET")) {
				webRequest.downloadHandler = new DownloadHandlerBuffer();
				yield return webRequest.SendWebRequest();
				var result = JsonUtility.FromJson<Leaderboard>(webRequest.downloadHandler.text);
				callback?.Invoke(result);
			}
		}

		public static IEnumerator SendScore(string name, int score, Action<PostScoreResult> callback) {
			using (var webRequest = new UnityWebRequest($"{apiUrlRoot}leaderboard", "POST")) {
				var data = new PostScore { name = name, score = score };
				webRequest.SetRequestHeader("Content-Type", "application/json");
				webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data)));
				webRequest.downloadHandler = new DownloadHandlerBuffer();
				yield return webRequest.SendWebRequest();

				var result = JsonUtility.FromJson<PostScoreResult>(webRequest.downloadHandler.text);
				callback?.Invoke(result);
			}
		}
	}
}