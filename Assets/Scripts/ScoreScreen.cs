using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    public Text scoreScreenText;

    void Start()
    {
        scoreScreenText.text = string.Format(
            "All your crops have died \n You have survived for {0} days \n your score is {1} \n" +
            "Made by Webox in 48hs for Ludum Dare 49 " +
            "Thanks for playing", PlayerPrefs.GetInt("days"), PlayerPrefs.GetInt("score"));
    }

}
