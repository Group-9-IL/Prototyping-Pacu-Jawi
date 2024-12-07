using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // Nama scene untuk loading screen
    public string loadingSceneName = "LoadingScreen";
    private Animator animator;
    
    void Start(){
        animator = GetComponent<Animator>();
        animator.SetTrigger("Start");
    }
    // Fungsi umum untuk memuat scene dengan loading screen
    public void LoadSceneWithLoading(string sceneToLoad)
    {
        StartCoroutine(LoadSceneCoroutine(sceneToLoad));
    }

    IEnumerator LoadSceneCoroutine(string sceneToLoad)
    {
        // Muat loading screen terlebih dahulu
        AsyncOperation loadLoadingScreen = SceneManager.LoadSceneAsync(loadingSceneName);
        while (!loadLoadingScreen.isDone)
        {
            Debug.Log("LoadingScene is loading: " + loadLoadingScreen.progress );
            // if (loadLoadingScreen.progress >= 0.9f){
            //     break;
            // }
            yield return null;
        }
        // Setelah loading screen selesai dimuat, mulai memuat scene tujuan
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneToLoad);
        loadScene.allowSceneActivation = false;
        // Tunggu sampai scene tujuan selesai dimuat
        while (!loadScene.isDone)
        {
            Debug.Log("Scene is loading: " + loadScene.progress * 100 + "%");
            if (loadScene.progress >= 0.5f)
            {
                loadScene.allowSceneActivation = true;
            }
            yield return null;
        }
        
    }
    
}
