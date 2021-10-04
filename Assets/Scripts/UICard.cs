using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICard : MonoBehaviour
{
    public AudioClip selectCard;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.transform.GetSiblingIndex());                
            }
        }
    }
    void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
    public void SelectCard()
    {
        playSound(selectCard);
        if (name.Contains("Movement"))
            GameObject.Find("CardsManager").GetComponent<CardsManager>().selectMovementCard(gameObject.transform.GetSiblingIndex());
        else if (name.Contains("Action"))
            GameObject.Find("CardsManager").GetComponent<CardsManager>().selectActionCard(gameObject.transform.GetSiblingIndex());

    }
}
