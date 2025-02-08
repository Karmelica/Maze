using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject creditsPanel;
    public GameObject optionsPanel;
    public GameObject levelSelectPanel;
    public GameObject mainPanel;
    
    public void Options()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void Credits()
    {   
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    
    public void LevelSelect()
    {
        mainPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }
    
    public void Back()
    {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        optionsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
    }
    
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
    
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
    }
}
