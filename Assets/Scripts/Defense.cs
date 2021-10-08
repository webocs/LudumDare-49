using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    public ClimateEvent.AOEShape canBlock;
    public int life;

    public void dealDamage(int damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Grid defGrid = GameObject.Find("DefensesGrid").GetComponent<Grid>();
            defGrid.RemoveFromGrid(defGrid.WorldToGrid(transform.position));
            Destroy(gameObject);
        }
    }
}
