using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float RestartWaitTime = 1f;
    [SerializeField] ParticleSystem Explosion;

    LevelManager levelManager;
    PlayerControls playerControls;
    
    void Awake() 
    {
        levelManager = FindObjectOfType<LevelManager>();
        playerControls = GetComponentInChildren<PlayerControls>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Pickup") {return;}
        else
        {
            StartCrashSequence();
        }
    }

    void StartCrashSequence()
    {
        Explosion.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;
        playerControls.enabled = false;
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(RestartWaitTime);
        levelManager.RestartLevel();
    }
}
