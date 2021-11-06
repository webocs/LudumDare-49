using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    public ClimateEvent.AOEShape canBlock;
    public int life;
    public AudioClip destroySfx;
    public GameObject shortDefenseDestroyedAnimation;

    public void dealDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Grid defGrid = GameObject.Find("DefensesGrid").GetComponent<Grid>();
            defGrid.RemoveFromGrid(defGrid.WorldToGrid(transform.position));
            PlaySound(destroySfx);
            Instantiate(shortDefenseDestroyedAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject,.4f);        
        }
    }

    void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
