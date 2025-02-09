using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private LevelLoader _levelLoader;
    public Transform player;
    public Slider bossHP;
    private int currentBossHP;
    private int maxBossHP = 100;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        _levelLoader = LevelLoader.instance;
        currentBossHP = maxBossHP;
    }

    private void OnDisable()
    {
        _levelLoader.lvl1Unlocked = true;
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        bossHP.value = currentBossHP;
        if(Input.GetKeyDown(KeyCode.E))
        {
            currentBossHP -= 2;
            if (currentBossHP <= 2)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
