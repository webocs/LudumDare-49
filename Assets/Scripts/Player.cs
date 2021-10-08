using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Movement SelectedMovement;
    public Action SelectedAction;

    public List<GameObject> MovementPreviewTiles;
    public GameObject movePreviewPrefab;
    public GameObject cantMovePreviewPrefab;

    public List<GameObject> ActionPreviewTiles;
    public GameObject actionPreviewPrefab;
    public GameObject cantActPreviewPrefab;

    public Grid grid;
    public Grid defenseGrid;

    public AudioClip moveSfx;
    public AudioClip placeDefense;    
    
    private void Start()
    {
        MovementPreviewTiles = new List<GameObject>();
        PreviewSelectedMovement();
        grid = GameObject.Find("CropsGrid").GetComponent<Grid>();
        defenseGrid = GameObject.Find("DefensesGrid").GetComponent<Grid>();
    }

    void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    public void ExecuteMovement(Vector2 newPosition)
    {
        
        if (!GameManager.GetInstance().gameOver)
            if (grid.IsInsideGridBounds(grid.WorldToGrid(newPosition)))
            {
                transform.position = newPosition;
                PreviewSelectedMovement();
                GameManager.GetInstance().Tic();
                playSound(moveSfx);
            }

    }

    public void PreviewSelectedMovement()
    {
        if (SelectedAction)
        {
            ClearSelectedAction();
        }

        if (SelectedMovement)
        {
            foreach (GameObject go in MovementPreviewTiles)
            {
                Destroy(go);
            }
            MovementPreviewTiles.Clear();
            foreach (Vector2Int v in SelectedMovement.PossibleDestinations)
            {
                Vector2 position = new Vector2(
                       v.x + transform.position.x,
                       v.y + transform.position.y);
                GameObject previewTile;
                if (grid.IsInsideGridBounds(grid.WorldToGrid(position)))                
                {
                    previewTile = Instantiate(
                                movePreviewPrefab,
                                position,
                                Quaternion.identity);
                }
                else
                {
                    previewTile = Instantiate(
                                  cantMovePreviewPrefab,
                                  position,
                                  Quaternion.identity);
                }               
                MovementPreviewTiles.Add(previewTile);
            }
        }
        else
        {
            foreach (GameObject go in MovementPreviewTiles)
            {
                Destroy(go);
            }
            MovementPreviewTiles.Clear();
        }
    }

    void ClearSelectedMovement()
    {
        SelectedMovement = null;
        foreach (GameObject go in MovementPreviewTiles)
        {
            Destroy(go);
        }
        MovementPreviewTiles.Clear();
    }

    void ClearSelectedAction()
    {
        SelectedAction = null;
        foreach (GameObject go in ActionPreviewTiles)
        {
            Destroy(go);
        }
        ActionPreviewTiles.Clear();
    }


    internal void PreviewSelectedAction()
    {
        if (SelectedMovement)
        {
            ClearSelectedMovement();
        }

        if (SelectedAction)
        {
            foreach (GameObject go in ActionPreviewTiles)
            {
                Destroy(go);
            }
            ActionPreviewTiles.Clear();

            foreach (Vector2Int v in SelectedAction.PossibleDestinations)
            {
                Vector2 position = new Vector2(
                       v.x + transform.position.x,
                       v.y + transform.position.y);
                GameObject previewTile;
                if (
                    SelectedAction.canBePlacedOnTopOfOtherDefenses || 
                    (grid.IsInsideGridBounds(grid.WorldToGrid(position)) && defenseGrid.GetObjectAt(grid.WorldToGrid(position)) ==null))
                {
                    previewTile = Instantiate(
                                actionPreviewPrefab,
                                position,
                                Quaternion.identity);
                }
                else
                {
                    previewTile = Instantiate(
                                  cantActPreviewPrefab,
                                  position,
                                  Quaternion.identity);
                }
                ActionPreviewTiles.Add(previewTile);
            }
        }
        else
        {
            foreach (GameObject go in ActionPreviewTiles)
            {
                Destroy(go);
            }
            ActionPreviewTiles.Clear();
        }
    }

    internal void ExecuteAction(Vector2 position)
    {
        Debug.Log("Executing action");
        if(!GameManager.GetInstance().gameOver)
            if (grid.IsInsideGridBounds(grid.WorldToGrid(position)))
            {
                GameObject actionObject = Instantiate(SelectedAction.placeablePrefab, position, Quaternion.identity);
                if(SelectedAction.takesSpaceInGrid)
                    defenseGrid.SetObjectAt(defenseGrid.WorldToGrid(position), actionObject);
                ClearSelectedAction();
                GameManager.GetInstance().Tic();
                playSound(placeDefense);
            }
    }
}
