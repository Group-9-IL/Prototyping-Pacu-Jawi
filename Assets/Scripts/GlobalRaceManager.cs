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
    public GameObject finishUI;
    public GameObject loseText;
    public GameObject winText;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool playerFinished = false;
    private bool botFinished = false;
    private bool matchFinished = false;

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
            return;
        } else
        {
            Time.timeScale = 1;
        }


        foreach (PlayerRaceManager player in listPlayerManager)
        {
            if(player.CompareTag("Bot") && player.GetRaceFinished())
            {
                botFinished = true;
            }

            if (player.CompareTag("Player") && player.GetRaceFinished())
            {
                playerFinished = true;
                finishUI.SetActive(true);
                if(!matchFinished)
                {
                    if (botFinished)
                    {
                        loseText.SetActive(true);
                        matchFinished = true ;
                    }
                    else
                    {
                        winText.SetActive(true);
                        matchFinished = true;
                    }
                }
            }
        }
    }
}
