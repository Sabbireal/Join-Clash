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

    float MaxLeft = -10;
    float MaxRight = 10;

    GameManager gameManager;
    PlayersManager playersManager;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        enemy = null;

        PlayersManager.maxLeft = MaxLeft;
        PlayersManager.maxRight = MaxRight;

        gameManager = FindObjectOfType<GameManager>();
        playersManager = FindObjectOfType<PlayersManager>();

        animator = GetComponent<Animator>();
        sprintngSpeed = gameManager.SprintngSpeed;
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
            PlayersManager.maxLeft = playersManager.gameObject.transform.position.x;
        }
        if (other.gameObject.name == "RightBorder")
        {
            PlayersManager.maxRight = playersManager.gameObject.transform.position.x;
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
        playersManager.UpdateAnimators();
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        Instantiate(gameManager.characterDestroyEffect, pos, Quaternion.identity);         
        Destroy(this.gameObject);
    }

    void OnCollideWithNeutral(GameObject neutral) {
        Debug.Log("Neutral Character Found");
        if (neutral.transform.parent.tag == "group")
        {
            IndividualCharacterController[] tempArray = neutral.transform.parent.gameObject.GetComponentsInChildren<IndividualCharacterController>();

            for (int i = 0; i < tempArray.Length; i++) {
                playersManager.addMember(tempArray[i].gameObject);
            }
        }
        else {
            playersManager.addMember(neutral);
        }
    }

    public void attackEnemy(GameObject enemy) {
        IndividualEnemyController individualEnemyController = enemy.GetComponent<IndividualEnemyController>();
        if (individualEnemyController.isAttacked == false) {
            individualEnemyController.isAttacked = true;
            this.enemy = enemy;
            isInControl = false;
            individualEnemyController.player = this.gameObject;

            transform.parent = null;
            playersManager.UpdateAnimators();

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
        this.gameObject.layer = 0;
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
