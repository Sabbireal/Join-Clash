using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndividualCharacterController : MonoBehaviour
{
    float MaxLeft = -3.5f;
    float MaxRight = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        MovementController.maxLeft = MaxLeft;
        MovementController.maxRight = MaxRight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "LeftBorder") 
        {
            MovementController.maxLeft = FindObjectOfType<MovementController>().gameObject.transform.position.x;
        }
        if (collision.gameObject.name == "RightBorder")
        {
            MovementController.maxRight = FindObjectOfType<MovementController>().gameObject.transform.position.x;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "LeftBorder")
        {
            MovementController.maxLeft = MaxLeft;
        }
        if (collision.gameObject.name == "RightBorder")
        {
            MovementController.maxRight = MaxRight;
        }
    }
}
