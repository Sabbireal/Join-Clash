using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UniversalController : MonoBehaviour
{
    GameObject players;
    public Animator[] animators;

    public static float maxLeft;
    public static float maxRight;

    public event Action allPlayerDead;
    public event Action onWin;

    float runningSpeed;

    // Start is called before the first frame update
    void Start()
    {
        players = this.gameObject;

        UpdateAnimators();
        UpdateisInControl();

        FindObjectOfType<GameManager>().startRunning += StartRunning;
        FindObjectOfType<GameManager>().playersMovement += MoveToLeftOrRight;
        FindObjectOfType<GameManager>().stopAnimOnPlayers += stopRunning;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isRunning) {
            players.transform.position = new Vector3(players.transform.position.x, 0.5f, players.transform.position.z + Time.deltaTime * runningSpeed);
        }
    }

    void StartRunning(float runningSpeed) {
        for (int i = 0; i < animators.Length; i++)
        {
            if (animators[i] == null) continue;
            animators[i].SetBool("IsRunning", true);
        }
        this.runningSpeed = runningSpeed;
    }

    void MoveToLeftOrRight(int dir, float speed)
    {
        if(dir != 0)
            players.transform.position = new Vector3(Mathf.Clamp(players.transform.position.x + dir * Time.deltaTime * speed, maxLeft, maxRight), 0.5f, players.transform.position.z);
        MoveToLeftOrRight_anim(dir);
    }

    void MoveToLeftOrRight_anim(int dir) {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetFloat("Direction", dir);
        }
    }

    void stopRunning() {
        Debug.Log("stopRunning");
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetFloat("Direction", 0);
            animators[i].SetBool("IsRunning", false);
            animators[i].Play("BasicMotions@Idle01");
        }
    }

    public void UpdateAnimators() {
        animators = GetComponentsInChildren<Animator>();

        if (animators.Length <= 0) {
            allPlayerDead?.Invoke(); //careful
        }
    }

    void UpdateisInControl() {
        for (int i = 0; i < animators.Length; i++) {
            animators[i].gameObject.GetComponent<IndividualCharacterController>().isInControl = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            GameManager.isCalculatingProgress = true;
        }
        
        if (other.gameObject.layer == 13)
        {
            onWin?.Invoke();
        }
    }

}
