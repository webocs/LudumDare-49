using UnityEngine;

public class Crop : MonoBehaviour
{
    public int currentLife=1;
    public int maxLife = 4;
    public int turnsUntilHarvest = 2;
    public Sprite[] lifeSprites;
    public int fullyGrownAtTurn;
    public bool isWatered=true;
    public bool isFullyGrown=false;
    public bool isAlive=false;

    private void Start()
    {       
        transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentLife];
        isWatered = true;
        isAlive = true;
    }

    private void Update()
    {
        if (!isFullyGrown)
        {
            if (currentLife >= maxLife)
            {
                isFullyGrown = true;
                fullyGrownAtTurn = GameManager.GetInstance().currentTurn;
            }
        }
        else if (currentLife < maxLife)
        {
            isFullyGrown = false;
            fullyGrownAtTurn = -1;
        }

        if (isFullyGrown && (GameManager.GetInstance().currentTurn - fullyGrownAtTurn) > turnsUntilHarvest)
            Harvest();
        if(currentLife>=0 && currentLife<maxLife)
            transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentLife];
    }

    internal void DealDamage(int damage)
    {        
        currentLife -= damage;        
        if (currentLife < 1) isAlive = false;      
        if (currentLife >= 0 && currentLife < maxLife)
            transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentLife];
        else
            Destroy(gameObject);

    }

    private void Harvest()
    {
        Destroy(this.gameObject);
    }

    internal void Grow()
    {
        Debug.Log("Growing");
        if (isWatered && !isFullyGrown)
            currentLife += 1;
    }
}
