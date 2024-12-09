using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using System.Collections;

public class SelectionManager : MonoBehaviour
{
    // Level Buttons
    public Button level1Button;  // Tambahkan untuk Level 1
    public Button level2Button;
    public Button level3Button;

    // Character Buttons
    public Button characterButton2;
    public Button characterButton3;
    // Image kunci
    public GameObject locked2Map;
    public GameObject locked3Map;
    public GameObject locked2Char;
    public GameObject locked3Char;

    private int selectedCharacter;
    public SceneLoader sceneLoader;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Memeriksa status terbuka atau terkunci dari PlayerPrefs
        CheckLevelProgress();
        CheckCharacterProgress();

        // Memastikan Level 1 selalu bisa dimainkan
        level1Button.interactable = true;

        // Muat pilihan karakter sebelumnya (jika ada)
        LoadSelections();
       
    }
    // Fungsi untuk memeriksa dan mengubah status level
    void CheckLevelProgress()
    {
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            level2Button.interactable = true; // Unlock level 2
            locked2Map.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Level2Completed", 0) == 1)
        {
            level3Button.interactable = true; // Unlock level 3
            locked3Map.SetActive(false);
        }
    }

    // Fungsi untuk memeriksa dan mengubah status karakter
    void CheckCharacterProgress()
    {
        if (PlayerPrefs.GetInt("Character1Selected", 0) == 1)
        {
            characterButton2.interactable = true; // Unlock Character 2
            locked2Char.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Character2Selected", 0) == 1)
        {
            characterButton3.interactable = true; // Unlock Character 3
            locked3Char.SetActive(false);
        }
    }

    // Fungsi untuk memuat pilihan karakter
    void LoadSelections()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0); // Default ke karakter 1
    }

    // Fungsi untuk memilih level dan memulai permainan
    public void SelectLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber);
        PlayerPrefs.Save();
        string levelSceneName = GetSceneNameForLevel(levelNumber);
        // Muat scene berdasarkan level yang dipilih
        if (sceneLoader != null)
        {
            sceneLoader.LoadSceneWithLoading(levelSceneName);
        }
        else
        {
            Debug.LogError("SceneLoader tidak ditemukan!");
        }
    }
    string GetSceneNameForLevel(int levelNumber)
    {
        switch (levelNumber)
        {
            case 1: return "RiceField";
            case 2: return "RiverSide";
            case 3: return "Mountain";
            default: return "Ricefield";
        }
    }

    // Fungsi untuk memilih karakter
    public void SelectCharacter(int characterNumber)
    {
        selectedCharacter = characterNumber;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
    }

    // Fungsi untuk mengupdate status level yang selesai
    public void CompleteLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("Level" + levelNumber + "Completed", 1);
        PlayerPrefs.Save();
    }
    public void SaveBotCharacterForLevel(int level)
    {
        PlayerPrefs.SetInt("Character" + level + "Selected", 1);
        PlayerPrefs.Save();
    }

}
