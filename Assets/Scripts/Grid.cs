using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Dictionary<Vector2Int,GameObject> _grid;
    public int Width;
    public int Height;
    public GameObject cropCell;
    public bool _showGrid = true;

    private void Start()
    {
        InitGrid();
    }

    public void InitGrid()
    {
        _grid = new Dictionary<Vector2Int, GameObject>();
        for(int x=0;x<Width;x++)
            for(int y = 0; y < Height; y++)
            {
                GameObject go = Instantiate(cropCell, new Vector3(x, y, transform.position.z),Quaternion.identity);
                go.GetComponent<Crop>().currentLife = Random.Range(0, 3);
                SetObjectAt(new Vector2Int(x, y), go);
            }
                
    }
    public bool IsInsideGridBounds(Vector2Int position)
    {
        return IsInsideGridBounds(position.x, position.y);
    }

    public bool IsInsideGridBounds(int col, int row)
    {
        return col >= 0 && col < Width && row >= 0 && row < Height;
    }

    public Vector2Int WorldToGrid(Vector2 position)
    {
        Vector3 mapRelative = transform.InverseTransformPoint(position);
        return new Vector2Int(Mathf.RoundToInt(mapRelative.x), Mathf.RoundToInt(mapRelative.y));
    }

    public Vector3 GridToWorld(Vector2Int position)
    {
        return new Vector3(
           transform.position.x + position.x,
           transform.position.y + position.y,
           0);
    }

    public GameObject GetObjectAt(Vector2Int position)
    {
        if (_grid.ContainsKey(position))
            return _grid[position];
        else
            return null;
    }

    public void SetObjectAt(Vector2Int position, GameObject go)
    {
        if (_grid.ContainsKey(position))
            _grid[position] = go;
        else
            _grid.Add(position, go);
    }

    void OnDrawGizmos()
    {       
        Color lineGizmoColor = new Color(0, 1f, .3f, 1f);

        // Draw a semitransparent blue cube at the transforms position
        if (_showGrid)
        {
            Gizmos.color = lineGizmoColor;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Gizmos.DrawWireCube(
                       new Vector3(transform.position.x + x, transform.position.y + y, 0),
                       new Vector3(1, 1, 0));
        }

    }


}
