using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GlobalRaceManager : MonoBehaviour
{

    public int lapTotal;
    public GameObject botCarPrefab;
    public List<Vector3> startPositon;
    public List<Quaternion> startRotation;
    public List<GameObject> playerCarPrefabs;
    public List<Transform> botWayPoints;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool playerFinished = false;
    private bool botFinished = false;

    void Awake()
    {   
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter",0);

        Instantiate(playerCarPrefabs[selectedCharacterIndex], startPositon[0], startRotation[0]);
        Instantiate(botCarPrefab, startPositon[1], startRotation[1]);

        foreach (PlayerRaceManager player in FindObjectsOfType<PlayerRaceManager>())
        {
            listPlayerManager.Add(player);
            player.SetLapTotal(lapTotal);
        }

    }

    void Update()
    {

        if (!TimerManager.Instance.getIsGameStarted())
        {
            Time.timeScale = 0;
            Debug.Log("belom");
            return;
        } else
        {
            Time.timeScale = 1;
        }

        //allPlayerFinished = true;

        //foreach (PlayerRaceManager player in listPlayerManager)
        //{
        //    if (!player.GetRaceFinished())
        //    {
        //        allPlayerFinished = false;
        //        break;
        //    }
        //}

        //if (allPlayerFinished)
        //{
        //    ForceStopRace();
        //}


        //if(TimerManager.Instance.getCurrentTime() > 300)
        //{
        //    ForceStopRace();
        //}
    }

    void ForceStopRace()
    {

    }
}
