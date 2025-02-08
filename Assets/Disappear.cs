using System;
using System.Collections;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
    
    private void Start()
    {
        StartCoroutine(DestroyObject());
    }
}
