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
            "You have survived for {0} days \n with a score of {1} \n" , PlayerPrefs.GetInt("days"), PlayerPrefs.GetInt("score"));
    }

}
