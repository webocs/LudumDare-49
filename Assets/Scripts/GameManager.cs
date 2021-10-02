using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentTurn=1;
    // Instancia estatica para el singleton
    private static GameManager _instance;
    public Text currentDayText;

    void Awake()
    {
        // Ya existe instancia?
        if (_instance == null)
        {
            // Asignarla a la variable estatica
            _instance = this;           
        }
        else
        {           
            Destroy(this.gameObject);
        }
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void Tic()
    {
        Debug.Log("TIC");
        currentTurn += 1;
        currentDayText.text = string.Format("Day {0}", currentTurn);
        Crop[] crops = FindObjectsOfType<Crop>();
        foreach(Crop c in crops){
            c.Grow();
        }
        Tickable[] tickables = FindObjectsOfType<Tickable>();
        foreach(Tickable tickable in tickables){
            tickable.Tic();
        }
    }
}
