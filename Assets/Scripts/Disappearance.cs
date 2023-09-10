using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappearance : MonoBehaviour
{
    public float timerDuration = 2.0f;
    public GameObject disappearingObject;
    
    private float _currentTime = 0.0f;
    private bool _isTimerRunning = false;
    private bool _isActive = true;

    private void Update()
    {
        if (_isTimerRunning)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= timerDuration)
            {
                _currentTime = 0.0f;
                disappearingObject.SetActive(!_isActive);
                _isActive = !_isActive;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _currentTime = 0.0f;
            _isTimerRunning = true;
        }
    }
}
