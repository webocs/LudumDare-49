using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortAnimation : MonoBehaviour
{
    public RuntimeAnimatorController animatorController;
    public float animationDuration;
    public float startIn;
        
    public void execute()
    {
        GetComponent<Animator>().runtimeAnimatorController = animatorController;
        GetComponent<Animator>().Play("Execute");
        Destroy(gameObject, animationDuration);
    }

    internal void Play()
    {
        Invoke("execute", startIn);
    }
}
