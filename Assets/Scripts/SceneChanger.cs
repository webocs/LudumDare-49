using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public void NextScene()
    {
        FindObjectOfType<Fader>().FadeIn();
        Invoke("change", 2f);
    }

    public void change() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restartgame()
    {
        FindObjectOfType<Fader>().FadeIn();
        Invoke("changeTo0", 2f);
    }
    public void changeTo0()
    {
        SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("days", 0);
        PlayerPrefs.SetInt("score", 0);
    }

    public void changeToTutorial()
    {
        SceneManager.LoadScene(3);       
    }
   public void changeToLeaderBoard()
    {
        SceneManager.LoadScene(4);       
    }

}
