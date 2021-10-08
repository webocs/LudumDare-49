using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndFade : MonoBehaviour
{

    public float moveSpeed;
    public float duration;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;    
    }
}
