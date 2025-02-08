using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioScript : MonoBehaviour
{
    static public AudioScript instance;
    public AudioClip[] musicClips;
    public AudioClip onButtonHover;
    public AudioClip onButtonClick;
    
    public AudioSource audioSource;
    
    public void PlayHoverSound()
    {
        audioSource.PlayOneShot(onButtonHover);
    }
    
    public void PlayClickSound()
    {
        audioSource.PlayOneShot(onButtonClick);
    }
    
    public void PlayMusic(int index)
    {
        audioSource.clip = musicClips[index];
        audioSource.Play();
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
            return;
        }
        
        PlayMusic(0);
        DontDestroyOnLoad(gameObject);
    }
}
