using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class PlayersManager : MonoBehaviour
{
    GameObject players;
    public Animator[] animators;

    public static float maxLeft;
    public static float maxRight;

    public event Action allPlayerDead;
    public event Action onWin;

    float runningSpeed;
    NavMeshPath navMeshPath;

    // Start is called before the first frame update
    void Start()
    {
        players = this.gameObject;
        navMeshPath = new NavMeshPath();

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
        if (dir != 0)
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

        if (other.gameObject.layer == 11) {
            OnCollideWithEnemyTrigger(other.transform.parent.gameObject);
        }
    }

    void OnCollideWithEnemyTrigger(GameObject enemy) {
        Debug.Log("Enemy Ahead");
        FindClosetPlayerFromEnemy(enemy).GetComponent<IndividualCharacterController>().attackEnemy(enemy);
    }
    GameObject FindClosetPlayerFromEnemy(GameObject enemy) {
        int playerNo = 0;
        float shortestDistance = 0;

        for (int i = 0; i < animators.Length; i++) {
            NavMeshAgent navMeshAgent = animators[i].transform.GetChild(2).gameObject.GetComponent<NavMeshAgent>();
            bool isAv = isPathAvailbale(navMeshAgent, enemy.transform);

            
            if (isAv) {
                if (i == 0) {
                    shortestDistance = getPathLength(navMeshPath);
                }
                else {
                    if (getPathLength(navMeshPath) < shortestDistance) {
                        playerNo = i;
                        shortestDistance = getPathLength(navMeshPath);
                    }
                }
            }
        }

        return animators[playerNo].gameObject;
    }
    bool isPathAvailbale(NavMeshAgent player, Transform EnemyPos) {
        player.CalculatePath(EnemyPos.position, navMeshPath);

        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else {
            return true;
        }
    }
    float getPathLength(NavMeshPath navMeshPath)
    {
        float length = 0.0f;

        if ((navMeshPath.status != NavMeshPathStatus.PathInvalid) && (navMeshPath.corners.Length > 1))
        {
            for (int i = 1; i < navMeshPath.corners.Length; ++i)
            {
                length += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
            }
        }

        return length;
    }

    public void addMember(GameObject potentialPlayer) {
        potentialPlayer.transform.parent = this.gameObject.transform;
        potentialPlayer.layer = 6;
        potentialPlayer.GetComponent<IndividualCharacterController>().isInControl = true;
        potentialPlayer.GetComponent<Animator>().SetBool("IsRunning", true);
        UpdateAnimators();
    }
}
