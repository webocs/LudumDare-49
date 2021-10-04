using UnityEngine;

public class Fader : MonoBehaviour
{

    #region SINGELTON_PATTERN
    public static Fader _instance;
    public static Fader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Fader>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("Fader");
                    _instance = container.AddComponent<Fader>();
                }
            }

            return _instance;
        }
    }
    #endregion

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }

    public void FadeOut()
    {
        animator.SetTrigger("fadeOut");
    }
}

