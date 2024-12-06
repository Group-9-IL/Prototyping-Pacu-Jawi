using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    // Level Buttons
    public Button level1Button;  // Tambahkan untuk Level 1
    public Button level2Button;
    public Button level3Button;

    // Character Buttons
    public Button characterButton2;
    public Button characterButton3;

    // Buffalo Buttons
    public Button buffaloButton2;
    public Button buffaloButton3;

    private int selectedCharacter;
    private int selectedBuffalo;

    void Start()
    {
        // Memeriksa status terbuka atau terkunci dari PlayerPrefs
        CheckLevelProgress();
        CheckCharacterProgress();
        CheckBuffaloProgress();

        // Memastikan Level 1 selalu bisa dimainkan
        level1Button.interactable = true;

        // Muat pilihan karakter dan buffalo sebelumnya (jika ada)
        LoadSelections();
    }

    // Fungsi untuk memeriksa dan mengubah status level
    void CheckLevelProgress()
    {
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
        {
            level2Button.interactable = true; // Unlock level 2
        }
        if (PlayerPrefs.GetInt("Level2Completed", 0) == 1)
        {
            level3Button.interactable = true; // Unlock level 3
        }
    }

    // Fungsi untuk memeriksa dan mengubah status karakter
    void CheckCharacterProgress()
    {
        if (PlayerPrefs.GetInt("Character1Selected", 0) == 1)
        {
            characterButton2.interactable = true; // Unlock Character 2
        }
        if (PlayerPrefs.GetInt("Character2Selected", 0) == 1)
        {
            characterButton3.interactable = true; // Unlock Character 3
        }
    }

    // Fungsi untuk memeriksa dan mengubah status buffalo
    void CheckBuffaloProgress()
    {
        if (PlayerPrefs.GetInt("Buffalo1Selected", 0) == 1)
        {
            buffaloButton2.interactable = true; // Unlock Buffalo 2
        }
        if (PlayerPrefs.GetInt("Buffalo2Selected", 0) == 1)
        {
            buffaloButton3.interactable = true; // Unlock Buffalo 3
        }
    }

    // Fungsi untuk memuat pilihan karakter dan buffalo
    void LoadSelections()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 1); // Default ke karakter 1
        selectedBuffalo = PlayerPrefs.GetInt("SelectedBuffalo", 1); // Default ke buffalo 1
    }

    // Fungsi untuk memilih level dan memulai permainan
    public void SelectLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("SelectedLevel", levelNumber);
        PlayerPrefs.Save();

        // Muat scene berdasarkan level yang dipilih
        SceneManager.LoadScene(levelNumber);
    }

    // Fungsi untuk memilih karakter
    public void SelectCharacter(int characterNumber)
    {
        selectedCharacter = characterNumber;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
    }

    // Fungsi untuk memilih buffalo
    public void SelectBuffalo(int buffaloNumber)
    {
        selectedBuffalo = buffaloNumber;
        PlayerPrefs.SetInt("SelectedBuffalo", selectedBuffalo);
        PlayerPrefs.Save();
    }

    // Fungsi untuk mengupdate status level yang selesai
    public void CompleteLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("Level" + levelNumber + "Completed", 1);
        PlayerPrefs.Save();
    }
}
