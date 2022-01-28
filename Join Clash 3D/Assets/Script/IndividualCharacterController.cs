using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndividualCharacterController : MonoBehaviour
{
    public bool isInControl = false;
    float sprintngSpeed = 0;
    bool goFight = false;
    GameObject enemy;

    float MaxLeft = -3.5f;
    float MaxRight = 3.5f;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enemy = null;

        PlayersManager.maxLeft = MaxLeft;
        PlayersManager.maxRight = MaxRight;

        animator = GetComponent<Animator>();
        sprintngSpeed = FindObjectOfType<GameManager>().SprintngSpeed;
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
            PlayersManager.maxLeft = FindObjectOfType<PlayersManager>().gameObject.transform.position.x;
        }
        if (other.gameObject.name == "RightBorder")
        {
            PlayersManager.maxRight = FindObjectOfType<PlayersManager>().gameObject.transform.position.x;
        }
        
        if (other.gameObject.layer == 9 && isInControl)
        {
            onCharacterDestroy();
        }
        else if (other.gameObject.layer == 8 && isInControl)
        {
            OnCollideWithNeutral(other.gameObject);
        }
        else if (other.gameObject.layer == 10 && !isInControl)
        {
            OnCollideWithEnemy();
        }
    }

    void onCharacterDestroy() {
        isInControl = false;
        transform.parent = null;
        FindObjectOfType<PlayersManager>().UpdateAnimators();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(FindObjectOfType<GameManager>().characterDestroyEffect, pos, Quaternion.identity);         
        Destroy(this.gameObject);
    }

    void OnCollideWithNeutral(GameObject neutral) {
        Debug.Log("Neutral Character Found");
        if (neutral.transform.parent.tag == "group")
        {
            IndividualCharacterController[] tempArray = neutral.transform.parent.gameObject.GetComponentsInChildren<IndividualCharacterController>();

            for (int i = 0; i < tempArray.Length; i++) {
                FindObjectOfType<PlayersManager>().addMember(tempArray[i].gameObject);
            }
        }
        else {
            FindObjectOfType<PlayersManager>().addMember(neutral);
        }
    }

    public void attackEnemy(GameObject enemy) {
        if (enemy.GetComponent<IndividualEnemyController>().isAttacked == false) {
            this.enemy = enemy;
            isInControl = false;
            enemy.GetComponent<IndividualEnemyController>().isAttacked = true;
            enemy.GetComponent<IndividualEnemyController>().player = this.gameObject;

            transform.parent = null;
            FindObjectOfType<PlayersManager>().UpdateAnimators();

            animator.SetFloat("Direction", 2);

            isInControl = false;
            goFight = true;
        }
    }

    void goToFight() {
        transform.LookAt(enemy.transform);
        transform.position += transform.forward * sprintngSpeed * Time.deltaTime;
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
            PlayersManager.maxLeft = MaxLeft;
        }
        if (other.gameObject.name == "RightBorder")
        {
            PlayersManager.maxRight = MaxRight;
        }
    }
}
