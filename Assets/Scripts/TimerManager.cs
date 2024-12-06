using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }

    private float currentGameTime;
    private bool isGameStarted;
    private float currentDelayTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentGameTime = 0f;
        currentDelayTime = 3f;
        isGameStarted = false;
    }
    private void Update()
    {
        if (!isGameStarted)
        {
            currentDelayTime -= Time.deltaTime;
            if(currentDelayTime < 0f)
            {
                isGameStarted = true;
            }
        }else
        {
            currentGameTime += Time.deltaTime;
        }
    }

    public float getCurrentTime()
    {
        return currentGameTime;
    }

    public bool getIsGameStarted()
    {
        return isGameStarted;
    }
}