using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public Animator animator;
    public Transform sword;
    public Transform player;
    public Transform attackPoint;
    public LineRenderer lineRenderer;
    
    private void OnEnable()
    {
        animator.enabled = false;
        sword.localPosition = new Vector3(0, -0.23f, 0.93f);
        sword.localRotation = Quaternion.Euler(70, 0, 0);
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, attackPoint.position);
    }

    private void Update()
    {
        if(attackPoint !=null)
        {
            lineRenderer.SetPosition(1, player.position);
        }
    }
}
