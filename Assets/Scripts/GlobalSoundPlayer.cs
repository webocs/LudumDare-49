using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundPlayer : MonoBehaviour
{

    public static GlobalSoundPlayer Instance { get => _instance; set => _instance = value; }
    private static GlobalSoundPlayer _instance;
    private void Awake()
    {      
        // Ya existe instancia?
        if (Instance == null)
        {
            // Asignarla a la variable estatica
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Play(AudioClip clip)
    {
        playSound(clip);
    }

    void playSound(AudioClip clip)
    {
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }

}
