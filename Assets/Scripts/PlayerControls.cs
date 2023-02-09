using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{   
    [Header("General Setup Settings:")]
    [Tooltip("How fast ship moves left/right")] [SerializeField] float moveSpeedX = 20f;
    [Tooltip("How fast ship moves up/down")] [SerializeField] float moveSpeedY = 20f;
    [Tooltip("Range on how far ship can fly left/right")] [SerializeField] float xRange = 10f;
    [Tooltip("Range on how far ship can fly up/down")] [SerializeField] float yRange = 7.5f;
    //[SerializeField] float yRangeAdjust = 2f; -- adjust yRange dynamically instead of via camera
   
    [Header("Flight Control based on screen position:")]
    [Tooltip("How fast nose of ship goes up/down, based on position")] [SerializeField] float positionPitchFactor = -3f;
    [Tooltip("How fast nose of ship goes left/right, based on position")] [SerializeField] float positionYawFactor = 3f;
    
    [Header("Flight Control based on player input:")]
    [Tooltip("How fast nose of ship goes up/down, based on control input")] [SerializeField] float controlPitchFactor = -12f;
    [Tooltip("How fast ship rolls left/right, based on control input")] [SerializeField] float controlRollFactor = 15f;
    [Tooltip("Inverts up/down controls")] [SerializeField] bool yIsInverted = false;
    
    [Header("Lasers Array:")]
    [SerializeField] GameObject[] lasers;

    float xThrow;
    float yThrow;
    float negativeY = 1;

    void Start()
    {
  
    }

    void Update()
    {
        InvertControls();
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void InvertControls()
    {
        if (yIsInverted)
        {
            negativeY = -1;
        }
        else
        {
            negativeY = 1;
        }
    }

    void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * moveSpeedX * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * moveSpeedY * Time.deltaTime * negativeY;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor * negativeY;
        
        float pitch =  pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.x * positionYawFactor;
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring()
    {
        if(Input.GetButton("Fire1"))
        {
            SetLasersActive(true);
        }   
        else
        {
            SetLasersActive(false);
        }
        
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
