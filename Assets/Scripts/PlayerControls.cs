using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{   
    [SerializeField] float moveSpeedX = 20f;
    [SerializeField] float moveSpeedY = 20f;
    [SerializeField] float xRange = 10f;
    [SerializeField] float yRange = 7.5f;
    //[SerializeField] float yRangeAdjust = 2f; -- adjust Range dynamically instead of via camera

    [SerializeField] float positionPitchFactor = -3f;
    [SerializeField] float controlPitchFactor = -12f;
    [SerializeField] float positionYawFactor = 3f;
    [SerializeField] float controlRollFactor = 15f;

    [SerializeField] bool yIsInverted = false;
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
}
