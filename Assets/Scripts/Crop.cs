using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject cropPrefab;

    public AudioClip hurtSfx;
    public AudioClip harvestSfx;

    public GameObject harvestAnimationPrefab;
    public bool wasHarvested;

    public GameObject shortHighlightAnimation;

    public GameObject scoreCanvas;

    private void Start()
    {       
        transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentLife];
        isWatered = true;
        isAlive = true;
        wasHarvested = false;
    }

    private void Update()
    {
        if (!wasHarvested)
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

           /* if (!wasHarvested && currentLife >= maxLife)
                Harvest();*/
            if (currentLife >= 0)
            {
                int currentSprite = currentLife - 1;
                if (currentSprite < 1) currentSprite = 1;
                if (currentSprite >= lifeSprites.Length) currentSprite = lifeSprites.Length - 1;
                transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentSprite];
            }
        }
    }

    internal void DealDamage(int damage)
    {        
        currentLife -= damage;        
        if (currentLife < 1) isAlive = false;
        if (currentLife >= 0 && currentLife < maxLife)
            transform.Find("sprite").GetComponent<SpriteRenderer>().sprite = lifeSprites[currentLife];
        else
        {
            Grid grid = GameObject.Find("CropsGrid").GetComponent<Grid>();
            grid.RemoveFromGrid(grid.WorldToGrid(transform.position));
            playSound(hurtSfx);
            Destroy(gameObject,.1f);
        }

    }

    void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    public void Harvest()
    {
        Grid grid = GameObject.Find("CropsGrid").GetComponent<Grid>();

        if (currentLife >= maxLife)
        {
            wasHarvested = true;
            Destroy(Instantiate(harvestAnimationPrefab, transform.position, Quaternion.identity), 1.2f);

            int addedScore = GameManager.GetInstance().IncreaseScore();            
            Vector2 upSlot = transform.position + Vector3.up;
            Vector2 downSlot = transform.position + Vector3.down;
            Vector2 leftSlot = transform.position + Vector3.left;
            Vector2 rightSlot = transform.position + Vector3.right;

            List<Vector2> slots = new List<Vector2>();
            slots.Add(upSlot);
            slots.Add(downSlot);
            slots.Add(leftSlot);
            slots.Add(rightSlot);

            if (grid.WorldToGrid(transform.position).x == 0) slots.Remove(leftSlot);
            else if (grid.WorldToGrid(transform.position).x == grid.Width-1) slots.Remove(rightSlot);
            if (grid.WorldToGrid(transform.position).y == 0) slots.Remove(downSlot);
            else if (grid.WorldToGrid(transform.position).y == grid.Height-1) slots.Remove(upSlot);
            

            // 50% chance of spawning 2 seeds instead of 1
            for (int i = 0; i < 2; i++)
            {
                Vector2 v = slots[Random.Range(0, slots.Count)];
                Vector2Int gridSlot = grid.WorldToGrid(v);
                if (grid.IsInsideGridBounds(gridSlot))
                {
                    if (grid.GetObjectAt(gridSlot) == null)
                    {
                        Crop c = Instantiate(cropPrefab, grid.GridToWorld(gridSlot), Quaternion.identity).GetComponent<Crop>();
                        Instantiate(shortHighlightAnimation, grid.GridToWorld(gridSlot), Quaternion.identity);
                        c.currentLife = 1;
                        grid.SetObjectAt(gridSlot, c.gameObject);
                    }
                }
                slots.Remove(v);
            }
            grid.RemoveFromGrid(grid.WorldToGrid(transform.position));
            GameObject canvas = Instantiate(scoreCanvas, transform.position, Quaternion.identity);
            canvas.GetComponentInChildren<Text>().text = addedScore + "";
            playSound(harvestSfx);
            Destroy(this.gameObject, .2f);
        }
        else
        {
            grid.RemoveFromGrid(grid.WorldToGrid(transform.position));
            playSound(harvestSfx);
            Destroy(this.gameObject, .2f);
        }
    }
   
    internal void Grow()
    {
        Debug.Log("Growing");
        if (isWatered && !isFullyGrown)
            currentLife += 1;
    }
}
