using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    public int maxMovementCards = 3;
    public int maxActionCards = 3;

    public int currentMovementCard;
    public Movement[] movementCards;
    public List<Movement> allMovements;


    public int currentActionCard;
    public Action[] actionCards;
    public List<Action> allActions;

    public GameObject uiMovementCardsHolderPanel;
    public GameObject uiActionCardsHolderPanel;
    public GameObject uiMovementCardPrefab;
    public GameObject uiActionCardPrefab;

    public Player player;

    void Start()
    {
        allActions = new List<Action>(FindObjectsOfType<Action>());
        actionCards = new Action[maxActionCards];
        for (int i = 0; i < maxActionCards; i++)
        {
            int randomCard = UnityEngine.Random.Range(0, allActions.Count);
            actionCards[i] = (allActions[randomCard]);
        }

        allMovements = new List<Movement>(FindObjectsOfType<Movement>());
        movementCards = new Movement[maxMovementCards];
        for (int i = 0; i < maxMovementCards; i++)
        {
           int randomCard = UnityEngine.Random.Range(0, allMovements.Count);
           movementCards[i]=(allMovements[randomCard]);
        }
        player = FindObjectOfType<Player>();
        UpdateCardsUI();
    }

    private void UpdateCardsUI()
    {
        foreach (Transform child in uiMovementCardsHolderPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in uiActionCardsHolderPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Movement movement in movementCards)
        {
            GameObject newCard = Instantiate(uiMovementCardPrefab, uiMovementCardsHolderPanel.transform);            
            newCard.transform.Find("CardSprite").GetComponent<Image>().sprite = movement.uiMovementSprite;
        }

        foreach (Action action in actionCards)
        {
            GameObject newCard = Instantiate(uiActionCardPrefab, uiActionCardsHolderPanel.transform);
            newCard.transform.Find("CardSprite").GetComponent<Image>().sprite = action.uiActionSprite;
        }
    }

    public Movement drawNewMovementCard() {
        int randomCard = UnityEngine.Random.Range(0, allMovements.Count);
        return (allMovements[randomCard]);        
    }

    private Action drawNewActionCard()
    {
        int randomCard = UnityEngine.Random.Range(0, allActions.Count);
        return (allActions[randomCard]);
    }
  
    public void selectMovementCard(int cardNumber)
    {
        if (cardNumber < movementCards.Length)
        {
            currentMovementCard = cardNumber;
            player.SelectedMovement = movementCards[cardNumber];
            player.PreviewSelectedMovement();
        }
    }
    public void selectActionCard(int cardNumber)
    {
        if (cardNumber < actionCards.Length)
        {
            currentActionCard = cardNumber;
            player.SelectedAction = actionCards[cardNumber];
            player.PreviewSelectedAction();
        }
    }

    public void useMovementCard(Vector2 position)
    {
        movementCards[currentMovementCard] = drawNewMovementCard();
        player.ExecuteMovement(position);
        player.SelectedMovement = null;
        player.PreviewSelectedMovement();
        UpdateCardsUI();
    }

    public void useActionCard(Vector2 position)
    {
        actionCards[currentMovementCard] = drawNewActionCard();
        player.ExecuteAction(position);
        player.SelectedAction = null;
        player.PreviewSelectedAction();
        UpdateCardsUI();
    }


    public void ExecuteMovement(GameObject go)
    {
        Vector3 position = go.transform.position;
        useMovementCard(position);
    }

    public void ExecuteAction(GameObject go)
    {
        Vector3 position = go.transform.position;
        useActionCard(position);
    }

    private void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectMovementCard(0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            selectMovementCard(1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            selectMovementCard(2);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectActionCard(0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            selectActionCard(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            selectActionCard(2);
        }
    }




}
