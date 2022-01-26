using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    GameObject players;
    Animator[] animators;

    public static float maxLeft;
    public static float maxRight;

    float runningSpeed;

    // Start is called before the first frame update
    void Start()
    {
        players = this.gameObject;
        animators = GetComponentsInChildren<Animator>();

        FindObjectOfType<GameManager>().startRunning += StartRunning;
        FindObjectOfType<GameManager>().playersMovement += MoveToLeftOrRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isRunning) {
            players.transform.position = new Vector3(players.transform.position.x, 0, players.transform.position.z + Time.deltaTime * runningSpeed);
        }
    }

    void StartRunning(float runningSpeed) {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("IsRunning", true);
        }
        this.runningSpeed = runningSpeed;
    }

    void MoveToLeftOrRight(int dir, float speed)
    {
        if(dir != 0)
            players.transform.position = new Vector3(Mathf.Clamp(players.transform.position.x + dir * Time.deltaTime * speed, maxLeft, maxRight), 0, players.transform.position.z);
        MoveToLeftOrRight_anim(dir);
    }

    void MoveToLeftOrRight_anim(int dir) {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetFloat("Direction", dir);
        }
    }

    void UpdateAnimators() {
        animators = GetComponentsInChildren<Animator>();
    }
}
