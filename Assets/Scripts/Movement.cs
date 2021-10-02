using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Vector2Int[] PossibleDestinations;
    public int maxWidth;
    public int maxHeight;
    public Sprite uiMovementSprite;
    private void Start()
    {
        // -1 to remove center
        PossibleDestinations = new Vector2Int[transform.Find("Positions").childCount];

        for (int i = 0; i < transform.Find("Positions").childCount;i++)
        {
            GameObject go = transform.Find("Positions").GetChild(i).gameObject;
            PossibleDestinations[i] = new Vector2Int((int)go.transform.localPosition.x, (int)go.transform.localPosition.y);
        }
    }

}
