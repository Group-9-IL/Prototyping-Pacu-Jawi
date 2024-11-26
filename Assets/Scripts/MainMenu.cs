using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void gameStart(){
        SceneManager.LoadScene("Mountain");
    }
    public void gameQuit(){
        Application.Quit();
    }
}
