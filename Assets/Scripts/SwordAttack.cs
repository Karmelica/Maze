using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public BossAttack bossAttack;
    private Animator _animator;
    private bool _firstSwipe = false;

    public void ChangeAttack()
    {
        _animator.SetBool("FirstSwipe", _firstSwipe);
        _firstSwipe = !_firstSwipe;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void HitEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Slicer>().SliceEnemy();
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger("Attack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            bossAttack.enabled = true;
            enabled = false;
        }
    }
}
