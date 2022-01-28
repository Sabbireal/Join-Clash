using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualEnemyController : MonoBehaviour
{
    public bool isAttacked = false;
    internal GameObject player;
    Animator animator;

    bool doRun = false;
    bool doOnce = true;

    float SprintngSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        SprintngSpeed = FindObjectOfType<GameManager>().SprintngSpeed;

        animator = this.GetComponent<Animator>();
        animator.SetBool("isFighting", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacked) {
            if (doOnce) {
                animator.SetFloat("Direction", 2);
                doOnce = false;
                doRun = true;
            }

        }

        if (doRun) {
            transform.LookAt(player.transform);
            transform.position += transform.forward * SprintngSpeed * Time.deltaTime;
        }
    }

    public void OnCollisionWithPlayer()
    {
        this.gameObject.layer = 0;
        Debug.Log("collision with player");
        doRun = false;
        animator.SetBool("IsDead", true);
    }
}
