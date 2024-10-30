using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GlobalRaceManager : MonoBehaviour
{

    public float raceTimer;
    public int lapTotal;    
    public int totalPlayer;
    public List<Vector3> startPositon;
    public List<Quaternion> startRotation;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool allPlayerFinished = false;

    void Start()
    {

        for(int i = 0;i < totalPlayer; i++)
        {

        }


        foreach (PlayerRaceManager player in FindObjectsOfType<PlayerRaceManager>())
        {
            listPlayerManager.Add(player);
            player.setLapTotal(lapTotal);
        }


    }

    void Update()
    {

        allPlayerFinished = true;

        foreach(PlayerRaceManager player in listPlayerManager)
        {
            if (!player.getRaceFinished())
            {
                allPlayerFinished = false;
                break;
            }
        }

        if (allPlayerFinished)
        {
            ForceStopRace();
        }


        raceTimer -= Time.deltaTime;
        if(raceTimer <= 0)
        {
            ForceStopRace();
        }   
    }

    void ForceStopRace()
    {

    }
}
