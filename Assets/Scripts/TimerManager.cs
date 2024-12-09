using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float currentGameTime;
    private bool isGameStarted;
    private float currentDelayTime;

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
            currentDelayTime -= Time.unscaledDeltaTime;
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
