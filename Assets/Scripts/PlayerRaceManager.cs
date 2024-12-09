using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerRaceManager : MonoBehaviour
{

    private int lapTotal;
    private int currentCheckPoint = 0;
    private int currentLap = 1;
    private bool raceFinished = false;
    private GameObject firstNumber;
    private GameObject secondNumber;
    private GameObject thirdNumber;


    private void Start()
    {
        if (CompareTag("Player"))
        {
            firstNumber = GameObject.Find("FirstNumber");
            secondNumber = GameObject.Find("SecondNumber");
            thirdNumber = GameObject.Find("ThirdNumber");

            if (firstNumber != null && secondNumber != null && thirdNumber != null)
            {
                firstNumber.SetActive(true);
                secondNumber.SetActive(false);
                thirdNumber.SetActive(false);
            }
        }
    }

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
                if(CompareTag("Player"))
                {
                    if(currentLap == 2)
                    {
                        firstNumber.SetActive(false);
                        secondNumber.SetActive(true);
                    }else if(currentLap == 3)
                    {
                        thirdNumber.SetActive(true);
                        secondNumber.SetActive(false);
                    }
                }
            }else
            {
                raceFinished = true;
            }
        }
    }

    public void SetLapTotal(int lap)
    {
        lapTotal = lap;
    }

    public bool GetRaceFinished()
    {
        return raceFinished;
    }
    // public void resetUI(){
    //     Debug.Log("Berhasil");
    //     firstNumber.SetActive(true);
    //     thirdNumber.SetActive(true);
    //     raceFinished =false;
    //     // currentLap = 1;
    //     // currentCheckPoint = 0;
    // }
}
