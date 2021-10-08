using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBucket : MonoBehaviour
{  
    void Start()
    {
        Grid grid = GameObject.Find("CropsGrid").GetComponent<Grid>();
        GameObject go = grid.GetObjectAt(grid.WorldToGrid(transform.position));
        if (go!=null && go.GetComponent<Crop>()!=null)
        {
            go.GetComponent<Crop>().currentLife += 1;
        }
        Destroy(gameObject);
        
    }

}
