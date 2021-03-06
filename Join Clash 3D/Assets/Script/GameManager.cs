using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public float runningSpeed = 5f;
    public float SprintngSpeed = 10f;
    public float movementSensitivity = 5f;

    [Header("Effect")]
    public GameObject characterDestroyEffect;

    [Header("Lines")]
    public Transform StartingLine;
    public Transform EndingLine;
    Transform players;
    float pathLength;
    public static bool isCalculatingProgress = false;

    [Header("Material")]
    public Material player_Mat;


    public event Action<float> startRunning;
    public event Action<int, float> playersMovement;
    public event Action<float> sentProgress;
    public event Action<bool, string, Color> sentResult;
    public event Action stopAnimOnPlayers;

    //UI
    public event Action showStartBtnEvent;
    public event Action showRestartBtnEvent;

    public static bool isRunning = false;
    bool isMovingLeftToRight = false;

    public static bool loadUISceneOnce = true;
    bool doOnce = true;

    UIManager uIManager;
    PlayersManager playersManager;

    private void Awake()
    {
        if (loadUISceneOnce)
        {
            SceneManager.LoadScene("Gameplay UI", LoadSceneMode.Additive);
            loadUISceneOnce = false;
        }
        else {
            UIManager.loadEvents = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectOfType<PlayersManager>().transform;
        pathLength = Vector3.Distance(StartingLine.position, EndingLine.position);

        uIManager = FindObjectOfType<UIManager>();
        playersManager = FindObjectOfType<PlayersManager>();

        uIManager.StartEvent += callStartRunning;
        uIManager.RestartEvent += callOnrestart;
        playersManager.allPlayerDead += CallOnAllPlayerDead;
        playersManager.onWin += callOnWin;
    }

    // Update is called once per frame
    void Update()
    {
        if (doOnce) { 
            showStartBtnEvent?.Invoke();
            doOnce = false;
        }

        if (!isRunning) return;

        if (isCalculatingProgress) calculateProgress();

        if (Input.GetKey(KeyCode.LeftArrow) && players.position.x > PlayersManager.maxLeft) {
            playersMovement?.Invoke(-1, movementSensitivity);
            isMovingLeftToRight = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && players.position.x < PlayersManager.maxRight){
            playersMovement?.Invoke(1, movementSensitivity);
            isMovingLeftToRight = true;
        }
        else if(isMovingLeftToRight)
        {
            isMovingLeftToRight = false;
            playersMovement?.Invoke(0, movementSensitivity);
        }
    }

    void callStartRunning() {
        Debug.Log("StartRunning");
        startRunning?.Invoke(runningSpeed);
        isRunning = true;
    }


    void callOnrestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void callOnWin() {
        StartCoroutine(onWin());
    }

    IEnumerator onWin() {
        yield return new WaitForSeconds(0.5f);
        isCalculatingProgress = false;
        sentResult?.Invoke(true, "Complete", Color.green);
        stopAnimOnPlayers?.Invoke();
        StartCoroutine(OnGameOver());
    }

    void CallOnAllPlayerDead()
    {
        StartCoroutine(OnGameOver("Failed", true));
    }

    IEnumerator OnGameOver(string resultTxt = null, bool sent = false) {
        isRunning = false;
        FindObjectOfType<CamController>().doRun = false;
        yield return new WaitForSeconds(2f);
        
        if(sent)
            sentResult?.Invoke(true, resultTxt, Color.red);
        
        showRestartBtnEvent?.Invoke();
    }

    void calculateProgress() {
        float alreadyCovered = Vector3.Distance(StartingLine.position, players.position);
        float progress = MathF.Round(alreadyCovered / pathLength, 3);

        sentProgress?.Invoke(progress);
    }
}
