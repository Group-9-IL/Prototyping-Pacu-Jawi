using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerRaceManager : MonoBehaviour
{

    private int lapTotal = 3;
    private int currentCheckPoint = 0;
    private int currentLap = 0;
    private bool raceFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Checkpoint_1") && currentCheckPoint == 0)
        {
            currentCheckPoint = 1;
        }

        if (other.gameObject.CompareTag("Checkpoint_2") && currentCheckPoint == 1)
        {
            currentCheckPoint = 2;
        }

        if (other.gameObject.CompareTag("Checkpoint_3") && currentCheckPoint == 2)
        {
            currentCheckPoint = 3;
        }

        if (other.gameObject.CompareTag("Line") && currentCheckPoint == 3)
        {
            if(currentLap < lapTotal)
            {
                currentLap++;
                currentCheckPoint = 0;
            }else
            {
                raceFinished = true;
            }
        }
    }

    public void setLapTotal(int lap)
    {
        lapTotal = lap;
    }

    public bool getRaceFinished()
    {
        return raceFinished;
    }
}
