using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalCover : MonoBehaviour
{
    public Vector2Int direction;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private void Start()
    {
        GameObject spritegameObject = transform.Find("sprite").gameObject; 
        if (direction == Vector2Int.up) spritegameObject.GetComponent<SpriteRenderer>().sprite = upSprite;
        else if (direction == Vector2Int.down) spritegameObject.GetComponent<SpriteRenderer>().sprite = downSprite;
        else  if (direction == Vector2Int.left) spritegameObject.GetComponent<SpriteRenderer>().sprite = leftSprite;
        else if (direction == Vector2Int.right) spritegameObject.GetComponent<SpriteRenderer>().sprite = rightSprite;
    }
}
