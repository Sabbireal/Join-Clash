using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndividualCharacterController : MonoBehaviour
{
    public bool isInControl = false;
    float SprintngSpeed = 0;
    bool goFight = false;
    GameObject enemy;

    float MaxLeft = -3.5f;
    float MaxRight = 3.5f;
    
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enemy = null;

        UniversalController.maxLeft = MaxLeft;
        UniversalController.maxRight = MaxRight;

        animator = GetComponent<Animator>();
        SprintngSpeed = FindObjectOfType<GameManager>().SprintngSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (goFight) {
            goToFight();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LeftBorder")
        {
            UniversalController.maxLeft = FindObjectOfType<UniversalController>().gameObject.transform.position.x;
        }
        if (other.gameObject.name == "RightBorder")
        {
            UniversalController.maxRight = FindObjectOfType<UniversalController>().gameObject.transform.position.x;
        }
        if (other.gameObject.layer == 9 && isInControl)
        {
            onCharacterDestroy();
        }

        if (other.gameObject.layer == 8 && isInControl)
        {
            OnCollideWithNeutral();
        }

        if (other.gameObject.layer == 11)
        {
            OnCollideWithEnemyTrigger(other.transform.parent.gameObject);
        }

        if (other.gameObject.layer == 10 && !isInControl)
        {
            OnCollideWithEnemy();
        }
    }

    void onCharacterDestroy() {
        isInControl = false;
        transform.parent = null;
        FindObjectOfType<UniversalController>().UpdateAnimators();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(FindObjectOfType<GameManager>().characterDestroyEffect, pos, Quaternion.identity);         
        Destroy(this.gameObject);
    }

    void OnCollideWithNeutral() { 
        
    }

    void OnCollideWithEnemyTrigger(GameObject enemy) {
        if (enemy.GetComponent<IndividualEnemyController>().isAttacked == false) {
            this.enemy = enemy;
            isInControl = false;
            enemy.GetComponent<IndividualEnemyController>().isAttacked = true;
            Debug.Log(this.gameObject.name);
            enemy.GetComponent<IndividualEnemyController>().player = this.gameObject;

            transform.parent = null;
            FindObjectOfType<UniversalController>().UpdateAnimators();

            animator.SetFloat("Direction", 2);

            isInControl = false;
            goFight = true;
        }
    }

    void goToFight() {
        transform.LookAt(enemy.transform);
        transform.position += transform.forward * SprintngSpeed * Time.deltaTime;
    }

    void OnCollideWithEnemy()
    {
        Debug.Log("Collided with enemy");
        goFight = false;
        animator.SetBool("IsDead", true);
        enemy.GetComponent<IndividualEnemyController>().OnCollisionWithPlayer();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "LeftBorder")
        {
            UniversalController.maxLeft = MaxLeft;
        }
        if (other.gameObject.name == "RightBorder")
        {
            UniversalController.maxRight = MaxRight;
        }
    }
}
