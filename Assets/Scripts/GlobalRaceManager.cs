using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GlobalRaceManager : MonoBehaviour
{
    public List<PlayerRaceManager> listPlayer = new List<PlayerRaceManager>();
    public float raceTimer = 300f;
    public int lapTotal = 3;
    
    void Start()
    {
        foreach (PlayerRaceManager player in FindObjectsOfType<PlayerRaceManager>())
        {
            listPlayer.Add(player);
            player.setLapTotal(lapTotal);
        }
    }

    void Update()
    {

        bool allPlayerFinished = true;

        foreach(PlayerRaceManager player in listPlayer)
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
