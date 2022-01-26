using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public float runningSpeed = 10f;
    public float movementSensitivity = 5f;
    
    public event Action<float> startRunning;
    public event Action<int, float> playersMovement;

    public static bool isRunning = false;
    bool isMovingLeftToRight = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            startRunning?.Invoke(runningSpeed);
            isRunning = true;
        }

        if (!isRunning) return;

        if (Input.GetKey(KeyCode.A)) {
            playersMovement?.Invoke(-1, movementSensitivity);
            isMovingLeftToRight = true;
        }
        else if (Input.GetKey(KeyCode.D)){
            playersMovement?.Invoke(1, movementSensitivity);
            isMovingLeftToRight = true;
        }
        else if(isMovingLeftToRight)
        {
            isMovingLeftToRight = false;
            playersMovement?.Invoke(0, movementSensitivity);
        }
    }
}
