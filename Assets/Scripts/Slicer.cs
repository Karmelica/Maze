using System;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    public GameObject playerCamera;
    
    public GameObject mainBody;
    public GameObject slicedBody1;
    public GameObject slicedBody2;

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Player"))
        {
            other.collider.GetComponent<Rigidbody>().AddForce((playerCamera.transform.position -transform.position).normalized * 15f, ForceMode.Impulse);
        }
    }

    public void SliceEnemy()
    {
        mainBody.SetActive(false);
        slicedBody1.transform.parent = null;
        slicedBody2.transform.parent = null;
        slicedBody1.SetActive(true);
        slicedBody1.GetComponent<Rigidbody>().AddForce(transform.forward * -5, ForceMode.Impulse);
        slicedBody2.SetActive(true);
        slicedBody2.GetComponent<Rigidbody>().AddForce(transform.forward * -5, ForceMode.Impulse);
        gameObject.SetActive(false);
    }
    
    private void Start()
    {
        mainBody.SetActive(true);
        slicedBody1.SetActive(false);
        slicedBody2.SetActive(false);
    }
    
    private void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerCamera.transform.position, 0.1f);
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void Update()
    {
        transform.LookAt(playerCamera.transform);
    }
    
}
