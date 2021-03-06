using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimateEvent : Tickable
{
    public enum AOEShape { VerticalLineUp, VerticalLineDown, HorizontalLineLeft, HorizontalLineRight, Circle};
    public int damage;
    public int aoeSize;
    public Vector2Int startingCell;
    public AOEShape shape;
    public Grid grid;
    public Grid defenseGrid;
    public int turnsUntilExecution;

    public GameObject climateEventPreview;
    public GameObject climateEventBlockedPreview;
    public List<GameObject> previewObjects;

    public AudioClip executionSfx;

    public GameObject shortAnimationPrefab;
    public RuntimeAnimatorController shortAnimationController;

    public bool alreadyExecuted;

    public Text turnsIndicator;

    public bool shakesCamera;
    private void Start()
    {
        previewObjects = new List<GameObject>();
        grid = GameObject.Find("CropsGrid").GetComponent<Grid>();
        defenseGrid = GameObject.Find("DefensesGrid").GetComponent<Grid>();
        startingCell = grid.WorldToGrid(transform.position);
        DrawEventPreview();
        alreadyExecuted = false;        
    }    
    
    void Update()
    {
        turnsIndicator.text = turnsUntilExecution+"";
        if (alreadyExecuted)
        {
            ClearPreview();            
            Destroy(gameObject, 2f);
            foreach(Transform t in turnsIndicator.transform.parent)
            {
                t.gameObject.SetActive(false);
            }

        }
    }

    public void ExecuteEvent()
    {
        alreadyExecuted = true;
        if (shakesCamera)
            Camera.main.GetComponent<CameraShake>().Shake();
        if (shape == AOEShape.VerticalLineUp)
        {
           
            for (int i = 0; i < aoeSize; i++)
            {
                
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x, startingCell.y + i);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.VerticalLineUp);
                if (defenseCheck == 1)
                    i = aoeSize;
                else if (defenseCheck == 2)
                    i++;
                else if (defenseCheck == 0)
                {

                    if (grid.IsInsideGridBounds(nextPosition))
                    {
                        g = grid.GetObjectAt(nextPosition);
                    }
                    if (g != null && g.GetComponent<Crop>())
                    {
                        g.GetComponent<Crop>().DealDamage(damage);
                    }
                    playShortAnimationInSeconds(nextPosition, i);
                }
                if (grid.WorldToGrid(FindObjectOfType<Player>().transform.position) == nextPosition)
                {
                    FindObjectOfType<Player>().DealDamage(1);
                }
                // ADd code for blockeers here
            }
        }
        else if (shape == AOEShape.VerticalLineDown)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x, startingCell.y - i);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.VerticalLineDown);
                if (defenseCheck == 1)
                    i = aoeSize;
                else if (defenseCheck == 2)
                    i++;
                else if (defenseCheck == 0)
                {

                    if (grid.IsInsideGridBounds(nextPosition))
                    {
                        g = grid.GetObjectAt(nextPosition);
                    }
                    if (g != null && g.GetComponent<Crop>())
                    {
                        g.GetComponent<Crop>().DealDamage(damage);
                    }
                    playShortAnimationInSeconds(nextPosition, i);
                }
                if (grid.WorldToGrid(FindObjectOfType<Player>().transform.position) == nextPosition)
                {
                    FindObjectOfType<Player>().DealDamage(1);
                }
                // ADd code for blockeers here
            }
        }
        else if (shape == AOEShape.HorizontalLineLeft)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x - i, startingCell.y);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.HorizontalLineLeft);
                if (defenseCheck == 1)
                    i = aoeSize;
                else if (defenseCheck == 2)
                    i++;
                else if (defenseCheck == 0)
                {

                    if (grid.IsInsideGridBounds(nextPosition))
                    {
                        g = grid.GetObjectAt(nextPosition);
                    }
                    if (g != null && g.GetComponent<Crop>())
                    {
                        g.GetComponent<Crop>().DealDamage(damage);
                    }
                    playShortAnimationInSeconds(nextPosition, i);
                }
                if (grid.WorldToGrid(FindObjectOfType<Player>().transform.position) == nextPosition)
                {
                    FindObjectOfType<Player>().DealDamage(1);
                }
                // ADd code for blockeers here
            }
        }
        else if (shape == AOEShape.HorizontalLineRight)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x + i, startingCell.y);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.HorizontalLineRight);
                if (defenseCheck == 1)
                    i = aoeSize;
                else if (defenseCheck == 2)
                    i++;
                else if (defenseCheck == 0)
                {

                    if (grid.IsInsideGridBounds(nextPosition))
                    {
                        g = grid.GetObjectAt(nextPosition);
                    }
                    if (g != null && g.GetComponent<Crop>())
                    {
                        g.GetComponent<Crop>().DealDamage(damage);
                    }
                    playShortAnimationInSeconds(nextPosition, i);
                }
                if (grid.WorldToGrid(FindObjectOfType<Player>().transform.position) == nextPosition)
                {
                    FindObjectOfType<Player>().DealDamage(1);
                }
                // ADd code for blockeers here
            }
        }
        else if (shape == AOEShape.Circle)
        {
            Debug.Log("Clearing Circle");
            for (var x = -aoeSize+1; x <= aoeSize-1; x++){
                for (var y = -aoeSize+1; y <= aoeSize-1; y++){
                    GameObject g = null;
                    Vector2Int nextPosition = new Vector2Int(startingCell.x + x, startingCell.y+y);
                    if (grid.IsInsideGridBounds(nextPosition))
                    {
                        g = grid.GetObjectAt(nextPosition);
                    }
                    if (g != null && g.GetComponent<Crop>())
                    {
                        g.GetComponent<Crop>().DealDamage(damage);
                    }
                    playShortAnimationInSeconds(nextPosition, x + y);
                    if (grid.WorldToGrid(FindObjectOfType<Player>().transform.position) == nextPosition)
                    {
                        FindObjectOfType<Player>().DealDamage(1);
                    }
                }
            }

        }
        ClearPreview();
        Destroy(gameObject, 20f);
    }

    private void playShortAnimationInSeconds(Vector2Int nextPosition, int i)
    {
        StartCoroutine(instantiateAnim(nextPosition, (float)i));
    }

    IEnumerator instantiateAnim(Vector2Int position, float seconds)
    {
        yield return new WaitForSeconds(seconds/10f);
        Destroy(Instantiate(shortAnimationPrefab, grid.GridToWorld(position), Quaternion.identity),1f);
        
    }
    private int CheckDefensesAtPosition(Vector2Int nextPosition, AOEShape shape,bool dealDamage=true)
    {
        int isblocked=0;
        if (defenseGrid.IsInsideGridBounds(nextPosition))
        {
            GameObject defense = defenseGrid.GetObjectAt(nextPosition);
            if (defense != null && defense.GetComponent<Defense>())
            {
                if (defense.GetComponent<Defense>().canBlock == shape)
                {                               
                    isblocked= 1;                                        
                }
                else if (defense.GetComponent<Defense>().canBlock == AOEShape.Circle)
                {                   
                    isblocked = 2;
                }
                if (dealDamage)
                    defense.GetComponent<Defense>().dealDamage(damage);
            }
        }
        return isblocked;
    }

    public void ClearPreview()
    {
        foreach (GameObject go in previewObjects)
        {
            Destroy(go);
        }
        previewObjects.Clear();
    }

    public void DrawEventPreview()
    {
        ClearPreview();
        if (shape == AOEShape.VerticalLineUp)
        {
            GameObject selectedPreview = climateEventPreview;
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x, startingCell.y + i);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.VerticalLineUp,false);
                if (defenseCheck == 2)
                {
                    previewObjects.Add(Instantiate(climateEventBlockedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
                else
                {
                    if (defenseCheck == 1)
                        selectedPreview = climateEventBlockedPreview;
                    previewObjects.Add(Instantiate(selectedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
            }
        }   
        else if (shape == AOEShape.VerticalLineDown)
        {
            GameObject selectedPreview = climateEventPreview;
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x, startingCell.y - i);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.VerticalLineDown, false);
                if (defenseCheck == 2)
                {
                    previewObjects.Add(Instantiate(climateEventBlockedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
                else
                {
                    if (defenseCheck == 1)
                        selectedPreview = climateEventBlockedPreview;
                    previewObjects.Add(Instantiate(selectedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
            }
        }
        else if( shape == AOEShape.HorizontalLineLeft)
        {
            GameObject selectedPreview = climateEventPreview;
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x - i, startingCell.y);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.HorizontalLineLeft, false);
                if (defenseCheck == 2)
                {
                    previewObjects.Add(Instantiate(climateEventBlockedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
                else
                {
                    if (defenseCheck == 1)
                        selectedPreview = climateEventBlockedPreview;
                    previewObjects.Add(Instantiate(selectedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
            }
        }
        else if (shape == AOEShape.HorizontalLineRight)
        {
            GameObject selectedPreview = climateEventPreview;
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x + i, startingCell.y);
                int defenseCheck = CheckDefensesAtPosition(nextPosition, AOEShape.HorizontalLineRight, false);
                if (defenseCheck == 2)
                {
                    previewObjects.Add(Instantiate(climateEventBlockedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
                else
                {
                    if (defenseCheck == 1)
                        selectedPreview = climateEventBlockedPreview;
                    previewObjects.Add(Instantiate(selectedPreview, grid.GridToWorld(nextPosition), Quaternion.identity));
                }
            }
        }
        else if (shape == AOEShape.Circle)
        {
            for (var x = -aoeSize + 1; x <= aoeSize - 1; x++)
            {
                for (var y = -aoeSize + 1; y <= aoeSize - 1; y++)
                {
                    Vector3 nextPosition = grid.GridToWorld(new Vector2Int(startingCell.x + x, startingCell.y + y));
                    previewObjects.Add(Instantiate(climateEventPreview, nextPosition, Quaternion.identity));
                }
            }
        }
    }

    public override void Tic()
    {
        turnsUntilExecution -= 1;
        if (turnsUntilExecution <= 0 && !alreadyExecuted)
        {
            alreadyExecuted = true;
            Debug.Log("Executing event");
            ExecuteEvent();
            GameManager.GetInstance().ClearClimateEvent(new Vector2Int((int)transform.position.x, (int)transform.position.y));
            playSound(executionSfx);
        }
        ClimateEvent[] events = FindObjectsOfType<ClimateEvent>();
        foreach(ClimateEvent c  in events)
        {
            c.DrawEventPreview();
        }
      
    }
    void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

}
