using System.Collections.Generic;
using UnityEngine;

public class ClimateEvent : Tickable
{
    public enum AOEShape { VerticalLineUp, VerticalLineDown, HorizontalLineLeft, HorizontalLineRight, Circle};
    public int damage;
    public int aoeSize;
    public Vector2Int startingCell;
    public AOEShape shape;
    public Grid grid;
    public int turnsUntilExecution;

    public GameObject climateEventPreview;
    public List<GameObject> previewObjects;
    private void Start()
    {
        previewObjects = new List<GameObject>();
        grid = FindObjectOfType<Grid>();
        startingCell = grid.WorldToGrid(transform.position);
        DrawEventPreview();
    }        

    public void ExecuteEvent()
    {
        if(shape == AOEShape.VerticalLineUp)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x, startingCell.y + i);
                if (grid.IsInsideGridBounds(nextPosition))
                {
                    g = grid.GetObjectAt(nextPosition);
                }
                if (g!=null && g.GetComponent<Crop>()) {
                    g.GetComponent<Crop>().DealDamage(damage);
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
                if (grid.IsInsideGridBounds(nextPosition))
                {
                    g = grid.GetObjectAt(nextPosition);
                }
                if (g != null && g.GetComponent<Crop>())
                {
                    g.GetComponent<Crop>().DealDamage(damage);
                }
                // ADd code for blockeers here
            }
        }
        else if (shape == AOEShape.HorizontalLineLeft)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                GameObject g = null;
                Vector2Int nextPosition = new Vector2Int(startingCell.x-i, startingCell.y);
                if (grid.IsInsideGridBounds(nextPosition))
                {
                    g = grid.GetObjectAt(nextPosition);
                }
                if (g != null && g.GetComponent<Crop>())
                {
                    g.GetComponent<Crop>().DealDamage(damage);
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
                if (grid.IsInsideGridBounds(nextPosition))
                {
                    g = grid.GetObjectAt(nextPosition);
                }
                if (g != null && g.GetComponent<Crop>())
                {
                    g.GetComponent<Crop>().DealDamage(damage);
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
                }
            }         
        }
        ClearPreview();
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
            for (int i = 0; i < aoeSize; i++)
            {
                Vector3 nextPosition = grid.GridToWorld(new Vector2Int(startingCell.x, startingCell.y + i));
                previewObjects.Add(Instantiate(climateEventPreview, nextPosition,Quaternion.identity));
            }
        }   
        else if (shape == AOEShape.VerticalLineDown)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                Vector3 nextPosition = grid.GridToWorld(new Vector2Int(startingCell.x, startingCell.y - i));
                previewObjects.Add(Instantiate(climateEventPreview, nextPosition, Quaternion.identity));
            }
        }
        else if (shape == AOEShape.HorizontalLineLeft)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                Vector3 nextPosition = grid.GridToWorld(new Vector2Int(startingCell.x - i, startingCell.y));
                previewObjects.Add(Instantiate(climateEventPreview, nextPosition, Quaternion.identity));
            }
        }
        else if (shape == AOEShape.HorizontalLineRight)
        {
            for (int i = 0; i < aoeSize; i++)
            {
                Vector3 nextPosition = grid.GridToWorld(new Vector2Int(startingCell.x + i, startingCell.y));
                previewObjects.Add(Instantiate(climateEventPreview, nextPosition, Quaternion.identity));
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
        if (turnsUntilExecution < 0)
        {
            Debug.Log("Executing event");
            ExecuteEvent();
        }
    }
}
