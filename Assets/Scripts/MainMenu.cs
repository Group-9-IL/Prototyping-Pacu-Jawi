using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject charSelect;
    public GameObject mapSelect;
    public GameObject credit;
    public GameObject setting;
    public GameObject exit;
    private Stack<GameObject> menuStack = new Stack<GameObject>();
    private void Start(){
        OpenMenu(mainMenu);
    }
    public void OpenMenu(GameObject newMenu){
        if (menuStack.Count > 0)
        {
            menuStack.Peek().SetActive(false);
        }
        newMenu.SetActive(true);
        menuStack.Push(newMenu);
    }
    public void gameQuit(){
        Application.Quit();
    }
    public void startButton(){
        OpenMenu(charSelect);
    }
    public void characterButton(){
        OpenMenu(mapSelect);
    }
    public void backButton(){
        if (menuStack.Count > 1)
        {
            menuStack.Pop().SetActive(false);
            menuStack.Peek().SetActive(true);
        }
    }
    public void creditButton(){
        OpenMenu(credit);
    }
    public void exitButton(){
        OpenMenu(exit);
    }
    public void settingButton(){
        OpenMenu(setting);
    }
}
