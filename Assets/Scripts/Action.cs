using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Sprite uiActionSprite;
    public string actionName;
    public GameObject placeablePrefab;
    public Grid grid;

    public Vector2Int[] PossibleDestinations;
    private void Start()
    {
        grid = FindObjectOfType<Grid>();

        // -1 to remove center
        PossibleDestinations = new Vector2Int[transform.Find("Positions").childCount];

        for (int i = 0; i < transform.Find("Positions").childCount; i++)
        {
            GameObject go = transform.Find("Positions").GetChild(i).gameObject;
            PossibleDestinations[i] = new Vector2Int((int)go.transform.localPosition.x, (int)go.transform.localPosition.y);
        }
    }

    public void executeAction(Vector3 worldPosition)
    {
        Vector2Int v2Position = grid.WorldToGrid(worldPosition);
        if (grid.IsInsideGridBounds(v2Position))
            Instantiate(placeablePrefab, grid.GridToWorld(v2Position),Quaternion.identity);
    }
}
