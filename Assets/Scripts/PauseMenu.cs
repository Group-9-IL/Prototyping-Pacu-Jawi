using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseUI;
    public GameObject gameUI;
    private SceneLoader sceneLoader;
    private void Start(){
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }
    public void Resume(){
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused=false;
    }
    public void Pause(){
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused=true;
    }
    public void backMenu(){
        Time.timeScale = 1f;
        sceneLoader.LoadSceneWithLoading("MainMenu");
    }
}
