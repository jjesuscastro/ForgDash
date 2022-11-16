using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector]
    public static UnityEvent onHit = new UnityEvent();
    [HideInInspector]
    public static UnityEvent onMiss = new UnityEvent();

    public AudioSource hitSFX;
    public AudioSource missSFX;
    public static int score;
    public static int combo;

    #region Singleton
    public static ScoreManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("[ScoreManager.cs] - Multiple ScoreManager(s) found!");
            Destroy(gameObject);
        }
    }
    #endregion 

    void Start()
    {
        score = 0;
        combo = 0;
    }

    public static void Hit()
    {
        score += 1;
        combo += 1;
        instance.hitSFX.Play();
        if (onHit != null)
            onHit.Invoke();
    }

    public static void Miss()
    {
        combo = 0;
        instance.missSFX.Play();
        if (onMiss != null)
            onMiss.Invoke();
    }
}