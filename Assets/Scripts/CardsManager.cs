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
    public ExecutableAction[] actionCards;
    public List<ExecutableAction> allActions;

    public GameObject uiMovementCardsHolderPanel;
    public GameObject uiActionCardsHolderPanel;
    public GameObject uiMovementCardPrefab;
    public GameObject uiActionCardPrefab;

    public Player player;

    private int scytheCardNumber = 0;

    void Start()
    {
        allActions = new List<ExecutableAction>(FindObjectsOfType<ExecutableAction>());
        int index = 0;
        foreach(ExecutableAction ea in allActions)
        {
            if (ea.placeablePrefab.name == "Scythe")
            {
                scytheCardNumber = index;
                break;
            }
            index++;
        }
        actionCards = new ExecutableAction[maxActionCards];
        for (int i = 0; i < maxActionCards; i++)
        {
            int randomCard = UnityEngine.Random.Range(0, allActions.Count);
            actionCards[i] = (allActions[randomCard]);
        }
        actionCards[UnityEngine.Random.Range(0, actionCards.Length)] = allActions[scytheCardNumber];

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

        foreach (ExecutableAction action in actionCards)
        {
            GameObject newCard = Instantiate(uiActionCardPrefab, uiActionCardsHolderPanel.transform);
            newCard.transform.Find("CardSprite").GetComponent<Image>().sprite = action.uiActionSprite;
        }
    }

    public Movement drawNewMovementCard() {
        int randomCard = UnityEngine.Random.Range(0, allMovements.Count);
        return (allMovements[randomCard]);        
    }

    private ExecutableAction drawNewActionCard()
    {
        bool hasScythe = false;
        int randomCard = UnityEngine.Random.Range(0, allActions.Count);
        foreach(ExecutableAction ea in actionCards)
        {
            if(ea.placeablePrefab.name == "Scythe")
            {
                hasScythe = true;
            }
        }
        if (!hasScythe)
        {
            Debug.Log("No scythe, adding one artificially");
            return allActions[scytheCardNumber];
        }
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
        actionCards[currentActionCard] = drawNewActionCard();
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


}
