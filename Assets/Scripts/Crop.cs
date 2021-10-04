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
    public GameObject cropPrefab;

    public AudioClip hurtSfx;
    public AudioClip harvestSfx;

    public GameObject harvestAnimationPrefab;
    public bool wasHarvested;

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

            if (!wasHarvested && currentLife >= maxLife)
                Harvest();
            if (currentLife >= 0)
            {
                int currentSprite = currentLife - 1;
                if (currentSprite < 1) currentSprite = 1;
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
            Destroy(gameObject,.5f);
        }

    }

    void playSound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    private void Harvest()
    {
        wasHarvested = true;
        Destroy(Instantiate(harvestAnimationPrefab, transform.position, Quaternion.identity),1.2f);
        
        GameManager.GetInstance().IncreaseScore();
        Vector2 upSlot = transform.position + Vector3.up;
        Vector2 downSlot = transform.position + Vector3.down;
        Vector2 leftSlot = transform.position + Vector3.left;
        Vector2 rightSlot = transform.position + Vector3.right;
        Vector2[] slots = { upSlot, downSlot, leftSlot, rightSlot };

        Grid grid = GameObject.Find("CropsGrid").GetComponent<Grid>();
        // 50% chance of spawning 2 seeds instead of 1
        for (int i = 0; i < (Random.Range(0,100)>85?1:2); i++)
        {
            Vector2 v = slots[Random.Range(0, slots.Length)];
            Vector2Int gridSlot = grid.WorldToGrid(v);
            if (grid.IsInsideGridBounds(gridSlot))
            {
                if (grid.GetObjectAt(gridSlot) == null)
                {
                    Crop c = Instantiate(cropPrefab, grid.GridToWorld(gridSlot), Quaternion.identity).GetComponent<Crop>();
                    c.currentLife = 1;
                    grid.SetObjectAt(gridSlot, c.gameObject);
                }
            }
        }      
        grid.RemoveFromGrid(grid.WorldToGrid(transform.position));
        playSound(harvestSfx);
        Destroy(this.gameObject,.2f);
    }
   
    internal void Grow()
    {
        Debug.Log("Growing");
        if (isWatered && !isFullyGrown)
            currentLife += 1;
    }
}
