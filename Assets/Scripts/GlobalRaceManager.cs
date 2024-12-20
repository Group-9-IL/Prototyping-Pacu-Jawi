using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private SelectionManager selectionManager;
    private PlayerRaceManager playerRaceManager;

    private List<PlayerRaceManager> listPlayerManager = new List<PlayerRaceManager>();
    private bool playerFinished = false;
    private bool botFinished = false;
    private bool matchFinished = false;
    private int currentLevel;
    private GameObject currentPlayerCar;
    private GameObject currentBotCar;
    private TimerManager timerManager;
    private AudioManager audioManager;

    void Awake()
    {   
        audioManager = FindObjectOfType<AudioManager>();
        playerRaceManager = FindObjectOfType<PlayerRaceManager>();
        timerManager = FindObjectOfType<TimerManager>();
        selectionManager = FindObjectOfType<SelectionManager>();
        Debug.Log(playerRaceManager);
        
        Debug.Log("sekarang di level" +currentLevel);
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter",0);

        currentPlayerCar=Instantiate(playerCarPrefabs[selectedCharacterIndex], startPositon[0], startRotation[0]);
        currentBotCar =Instantiate(botCarPrefab, startPositon[1], startRotation[1]);
        foreach (PlayerRaceManager player in FindObjectsOfType<PlayerRaceManager>())
        {
            listPlayerManager.Add(player);
            player.SetLapTotal(lapTotal);
        }

    }

    void Update()
    {

        if (!timerManager.getIsGameStarted()||PauseMenu.gameIsPaused)
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
                        audioManager.PlaySFX(SfxCondition.failed);
                        audioManager.PlaySFX(SfxCondition.boo);
                        loseText.SetActive(true);
                        matchFinished = true ;
                    }
                    else
                    {
                        audioManager.PlaySFX(SfxCondition.victory);
                        audioManager.PlaySFX(SfxCondition.celebration);
                        winText.SetActive(true);
                        matchFinished = true;
                        int currentLevel = SceneManager.GetActiveScene().buildIndex;
                        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
                        PlayerPrefs.Save();
                        Debug.Log("LevelSekarang"+currentLevel);
                        selectionManager.CompleteLevel(currentLevel);
                        selectionManager.SaveBotCharacterForLevel(currentLevel);
                        Debug.Log("Level " + currentLevel + " Completed: " + PlayerPrefs.GetInt("Level" + currentLevel + "Completed"));

                    }
                }
            }
        }
    }
    public void ResetRace()
    {
     SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }
}
