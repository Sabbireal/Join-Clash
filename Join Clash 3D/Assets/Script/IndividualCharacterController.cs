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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LeftBorder")
        {
            MovementController.maxLeft = FindObjectOfType<MovementController>().gameObject.transform.position.x;
        }
        if (other.gameObject.name == "RightBorder")
        {
            MovementController.maxRight = FindObjectOfType<MovementController>().gameObject.transform.position.x;
        }

        if (other.gameObject.layer == 9)
        {
            onCharacterDestroy();
        }
    }

    void onCharacterDestroy() {
        transform.parent = null;
        FindObjectOfType<MovementController>().UpdateAnimators();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(FindObjectOfType<GameManager>().characterDestroyEffect, pos, Quaternion.identity);         
        Destroy(this.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftBorder")
        {
            MovementController.maxLeft = MaxLeft;
        }
        if (other.gameObject.name == "RightBorder")
        {
            MovementController.maxRight = MaxRight;
        }
    }
}
