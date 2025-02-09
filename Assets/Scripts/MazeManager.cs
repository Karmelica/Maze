using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    private LevelLoader _levelLoader;
    public GameObject cutsceneTrigger;
    
    private void Start()
    {
        _levelLoader = LevelLoader.instance;
        if(_levelLoader !=null)
        {
            if (_levelLoader.lvl1Unlocked)
            {
                cutsceneTrigger.SetActive(false);
            }
        }
    }
}
