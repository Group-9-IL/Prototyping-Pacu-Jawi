using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GlobalRaceManager : MonoBehaviour
{

    public int lapTotal;    
    public int totalPlayer;
    public List<Vector3> startPositon;
    public List<Quaternion> startRotation;
    public List<GameObject> carPrefabs;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool allPlayerFinished = false;

    void Start()
    {

        for(int i = 0;i < totalPlayer; i++)
        {
            Instantiate(carPrefabs[i], startPositon[i], startRotation[i]);
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


        if(TimerManager.Instance.getCurrentTime() > 300)
        {
            ForceStopRace();
        }
    }

    void ForceStopRace()
    {

    }
}
