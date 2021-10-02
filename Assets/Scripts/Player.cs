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

    private void Start()
    {
        MovementPreviewTiles = new List<GameObject>();
        PreviewSelectedMovement();
        grid = FindObjectOfType<Grid>();
    }


    public void ExecuteMovement(Vector2 newPosition)
    {
       
        if (grid.IsInsideGridBounds(grid.WorldToGrid(newPosition)))
        {
            transform.position = newPosition;
            PreviewSelectedMovement();
            GameManager.GetInstance().Tic();
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
                if (grid.IsInsideGridBounds(grid.WorldToGrid(position)))
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
        if (grid.IsInsideGridBounds(grid.WorldToGrid(position)))
        {
            GameObject actionObject = Instantiate(SelectedAction.placeablePrefab, position, Quaternion.identity);
            ClearSelectedAction();
            GameManager.GetInstance().Tic();
        }
    }
}