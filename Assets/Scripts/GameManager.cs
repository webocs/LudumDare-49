using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentTurn=1;
    public int currentScore=0;
    public int cropScore=0;
    // Instancia estatica para el singleton
    private static GameManager _instance;
    public Text currentDayText;
    public Text currentScoreText;
    public ClimateEvent[] climateEvents;
    public Grid cropsGrid;
    public Dictionary<Vector2Int, ClimateEvent> currentClimateEvents;
    public int ClimateEventSpawnChance;
    public bool gameOver=false;

    void Awake()
    {
        currentClimateEvents = new Dictionary<Vector2Int, ClimateEvent>();
        cropsGrid = GameObject.Find("CropsGrid").GetComponent<Grid>();
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
        currentScoreText.text = string.Format("{0}", currentScore);

        Crop[] crops = FindObjectsOfType<Crop>();
        foreach(Crop c in crops){
            c.Grow();
        }
        Tickable[] tickables = FindObjectsOfType<Tickable>();
        foreach(Tickable tickable in tickables){
            tickable.Tic();
        }
        if(UnityEngine.Random.Range(0,100)< ClimateEventSpawnChance)
            SpawnClimateEvent();

        if (cropsGrid.elementsInGrid <= 0)
        {
            GameOver();
        }
    }
    private void Update()
    {
        ClimateEventSpawnChance = Mathf.CeilToInt(Mathf.Log(currentTurn)* 15);
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over");
        PlayerPrefs.SetInt("days", currentTurn);
        PlayerPrefs.SetInt("score", currentScore);
        FindObjectOfType<SceneChanger>().change();
    }

    void SpawnClimateEvent()
    {
        GameObject go =climateEvents[UnityEngine.Random.Range(0, climateEvents.Length)].gameObject;
        bool isHorizontal = UnityEngine.Random.Range(0, 100) > 50;
        bool isOnTopOrLeft = UnityEngine.Random.Range(0, 100) > 50;
        int xValue = 0;
        int yValue = 0;
        if (isHorizontal && isOnTopOrLeft) yValue = cropsGrid.Height-1;
        if (!isHorizontal && !isOnTopOrLeft) xValue = cropsGrid.Width-1;
        if (isHorizontal)
        {
            xValue = UnityEngine.Random.Range(0, cropsGrid.Width);
        }
        else
        {
            yValue = UnityEngine.Random.Range(0, cropsGrid.Height);
        }
        ClimateEvent eventToSpawn = null;
        foreach(ClimateEvent c in climateEvents)
        {
            if (isHorizontal && !isOnTopOrLeft)
            {
                if (c.shape == ClimateEvent.AOEShape.VerticalLineUp)
                {
                    eventToSpawn = c;
                    break;
                }
            }
            else if (isHorizontal && isOnTopOrLeft)
            {
                if (c.shape == ClimateEvent.AOEShape.VerticalLineDown)
                {
                    eventToSpawn = c;
                    break;
                }
            }
            else if (!isHorizontal && isOnTopOrLeft)
            {
                if (c.shape == ClimateEvent.AOEShape.HorizontalLineRight)
                {
                    eventToSpawn = c;
                    break;
                }
            }
            else
            {
                if (c.shape == ClimateEvent.AOEShape.HorizontalLineLeft)
                {
                    eventToSpawn = c;
                    break;
                }
            }
        }
        if (eventToSpawn != null)
        {
            Vector2Int position = new Vector2Int(xValue, yValue);
            if (!currentClimateEvents.ContainsKey(position) || !currentClimateEvents[position]!=null)
            {
               currentClimateEvents.Add(
                    position, Instantiate(eventToSpawn.gameObject,
                    cropsGrid.GridToWorld(position), Quaternion.identity).GetComponent<ClimateEvent>()
                );
            }
        }
    }

    internal void IncreaseScore()
    {
        currentScore = Mathf.CeilToInt(10f* Mathf.Log(currentTurn) * cropScore);
    }

    public void ClearClimateEvent(Vector2Int position)
    {
        if (currentClimateEvents.ContainsKey(position))
            currentClimateEvents.Remove(position);
    }
}
