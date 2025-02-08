using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public Slider bossHP;
    private int currentBossHP;
    private int maxBossHP = 100;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        currentBossHP = maxBossHP;
    }

    // Update is called once per frame
    void Update()
    {
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
