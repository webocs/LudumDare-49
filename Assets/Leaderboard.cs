using Models;
using Proyecto26;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
	public string basePath = "https://zife2s.deta.dev";
	public string playerName;
	public InputField inputField;
	public GameObject leaderboardPanel;
	public GameObject leaderboardEntry;

	private void Start()
	{
		InitializeLeaberboard();
	}

	private void InitializeLeaberboard()
	{		
		RequestHelper requestOptions = null;
	
		RestClient.Get<Scores>(basePath + "/score")
			.Then(res => {
				foreach (Transform t in leaderboardPanel.transform)
				{
					Destroy(t.gameObject);
				}
				Debug.Log("Score");
				Score[] scores = res.scores;
				Array.Sort(scores, new ScoreComparer());
				int i = 1;
				foreach(Score s in scores)
				{
					GameObject ldEntry = Instantiate(leaderboardEntry, leaderboardPanel.transform);
					ldEntry.GetComponent<Text>().text = string.Format("{0}.{1} - {2}",i, s.name, s.score);
					Debug.Log(s.score);
					i++;
				}				
			})
			.Catch(err => Debug.Log( String.Format("Error {0}", err.Message)));
	}

	public void SubmitScore()
    {
		GameObject.Find("SubmitButton").GetComponent<Button>().enabled = false;
		playerName = inputField.text;
		if (playerName == "") playerName = "Anonymous";
		Post();
		InitializeLeaberboard();
	}
	public void Post()
	{
		RequestHelper currentRequest = new RequestHelper
		{
			Uri = basePath + "/baddaytoharvest/score",
			Headers = new Dictionary<string, string> {
				{ "Api-key", "" }
			},
			Body = new Score
			{
				name = playerName,
				value = PlayerPrefs.GetInt("score")
			},
			EnableDebug = true
		};
		RestClient.Post<Score>(currentRequest)
		.Then(res => {
				// And later we can clear the default query string params for all requests
				RestClient.ClearDefaultParams();
			Debug.Log( string.Format("Success", JsonUtility.ToJson(res, true)));
		})
		.Catch(err => Debug.Log(string.Format("Error {0}", err.Message)));
	}

	public void GetLeaderboard()
	{

	}
}


[Serializable]
public class Score
{
	public int score;
	public int value;
	public string name;

	public override string ToString()
	{
		return UnityEngine.JsonUtility.ToJson(this, true);
	}
}


[Serializable]
public class Scores
{
	public Score[] scores;
	public override string ToString()
	{
		return UnityEngine.JsonUtility.ToJson(this, true);
	}
}

class ScoreComparer : IComparer
{
	public int Compare(object x, object y)
	{
		Score scoreX = (Score)x;
		Score scoreY = (Score)y;
		return (scoreY.score - scoreX.score); 
	}
}
