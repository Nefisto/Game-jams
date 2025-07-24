
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public readonly struct HighScore
{
	public readonly string userName;
	public readonly int score;

	public HighScore (string userName, int score)
	{
		this.userName = userName;
		this.score = score;
	}

	public override string ToString ()
	{
		return $"User: {userName}\tScore: {score}";
	}
}

public class LeaderboardUserUI
{
	public TextMeshProUGUI username;
	public TextMeshProUGUI score;
}

public class EndScreenScoreManager : MonoBehaviour
{
	private const string privateCode = "NCIb6TaTfEm1zEJgI_WQgAcyJkm2nIW0O7N6aahnYGRQ";
	private const string publicCode = "5ff830370af26924d0328858";
	private const string webURL = "http://dreamlo.com/lb/";

	private List<HighScore> leaderScores = new List<HighScore>(); // To verify if name is already taken
	
	public List<GameObject> leaderBoardUsersObjects;
	private List<LeaderboardUserUI> leaderboardUI = new List<LeaderboardUserUI>();
	
	// public TMP_InputField
	private void Awake ()
	{
		foreach (var user in leaderBoardUsersObjects)
		{
			var nameChild = user.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
			var pointChild = user.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

			nameChild.text = "null";
			pointChild.text = "llun";
			
			leaderboardUI.Add(new LeaderboardUserUI(){username = nameChild, score = pointChild});
		}
	}

	private IEnumerator UpdateUI ()
	{
		yield return UpdateLeaderBoard();

		for (int i = 0; i < leaderScores.Count; i++)
		{
			leaderboardUI[i].username.text = leaderScores[i].userName;
			leaderboardUI[i].score.text = leaderScores[i].score.ToString();
		}
	}
	
	// POST
	public void TryAddNewHighScore (string username, int score)
	{
		StartCoroutine(UploadNewHighScore());
		
		IEnumerator UploadNewHighScore ()
		{
			yield return UpdateLeaderBoard(); // Refresh users in leaderboard
			
			// Is username already taken?
			bool isRepeatedUsername = !leaderScores.IsNullOrEmpty() && leaderScores.Exists(highScore
				=> string.Equals(highScore.userName, username, StringComparison.OrdinalIgnoreCase));

			if (isRepeatedUsername)
			{
				Debug.Log("Sorry! This username already exist in leaderboard");

				yield break;
			}
			
			using (var webRequest = new UnityWebRequest($"{webURL}{privateCode}/add/{username}/{score}"))
			{
				yield return webRequest.SendWebRequest();
				
				if (webRequest.isNetworkError || webRequest.isHttpError)
					Debug.Log($"Something goes wrong when updating score: {webRequest.error}");
				else
					Debug.Log("Highscore updated!");
			}
		}
	}
	
	// GET
	// public void UpdateLeaderBoard ()
	// {
	// 	
	// 	StartCoroutine(_UpdateLeaderBoard());
	//
	// 	
	// }
	
	private IEnumerator UpdateLeaderBoard ()
	{
		leaderScores.Clear();
			
		using (var webRequest = UnityWebRequest.Get(webURL + publicCode + "/pipe/"))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError || webRequest.isHttpError)
				Debug.Log($"Something goes wrong when requesting highscores: {webRequest.error}");
			else
			{
				leaderScores = FormatStream(webRequest.downloadHandler.text).ToList(); // update LOCAL leaderboard

				leaderScores.ForEach(x => Debug.Log(x));
			}
		}
		
		IEnumerable<HighScore> FormatStream (string scoreStream)
		{
			var entries = scoreStream.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries); // Separate for each user
			var formattedStream = new List<HighScore>();

			foreach(var entry in entries)
			{
				var formattedUser = entry
					.Split(new[] {'|'}, StringSplitOptions.None)
					.ToArray();

				Debug.Log(formattedUser);
				formattedStream.Add(new HighScore(formattedUser[0], int.Parse(formattedUser[1])));
			}

			return formattedStream;
		}
	}
}