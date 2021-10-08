using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        Grid cropsGrid = GameObject.Find("CropsGrid").GetComponent<Grid>();
        GameObject go = cropsGrid.GetObjectAt(cropsGrid.WorldToGrid(transform.position));
        if (go)
        {
            Crop crop = go.GetComponent<Crop>();
            if (crop)
            {
                crop.Harvest();
            }
        }
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
