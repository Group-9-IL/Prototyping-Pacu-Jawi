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
    public List<Transform> botWayPoints;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool allPlayerFinished = false;

    void Awake()
    {   
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter",0);
        Instantiate(carPrefabs[selectedCharacterIndex],startPositon[0],startRotation[0]);
        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1); // Ambil level yang dipilih
        int botCharacter = PlayerPrefs.GetInt("BotCharacterForLevel" + currentLevel, 0);
        if (botCharacter >= 0 && botCharacter < carPrefabs.Count)
        {
        Instantiate(carPrefabs[botCharacter], startPositon[1], startRotation[1]);
        }

        foreach (PlayerRaceManager player in FindObjectsOfType<PlayerRaceManager>())
        {
            listPlayerManager.Add(player);
            player.SetLapTotal(lapTotal);
        }


    }

    void Update()
    {

        allPlayerFinished = true;

        foreach(PlayerRaceManager player in listPlayerManager)
        {
            if (!player.GetRaceFinished())
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
