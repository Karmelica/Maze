using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    static public LevelLoader instance;
    public bool lvl1Unlocked = false;
    public bool lvl2Unlocked = false;
    public Slider loadingBar;
    public GameObject loadingScreen;
    
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }
    
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadLevelAsync(sceneIndex));
    }

   private IEnumerator LoadLevelAsync(int sceneIndex)
   {
       PlayerController.canBeControlled = false;
        loadingScreen.SetActive(true);
    
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
    
        float elapsedTime = 0f;
        while (!operation.isDone)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / 2f);
            loadingBar.value = progress;
    
            if (elapsedTime >= 2f)
            {
                operation.allowSceneActivation = true;
            }
    
            yield return null;
        }
    
        PlayerController.canBeControlled = true;
        loadingScreen.SetActive(false);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}