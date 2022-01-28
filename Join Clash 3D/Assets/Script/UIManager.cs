using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject Start_Btn;
    public GameObject Restart_Btn;
    public GameObject Progress_Slider;
    public GameObject resutlTxt;
    public GameObject Canvas;

    public event Action StartEvent;
    public event Action RestartEvent;

    public static bool loadEvents = false;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("UI Manager Loaded");
        subscribe();

        Canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }


    private void Update()
    {
        if (loadEvents) {
            subscribe();
            loadEvents = false;
        }
    }

    void subscribe() {
        Debug.Log("UI Manager Subscribed");

        gameManager = FindObjectOfType<GameManager>();

        gameManager.showStartBtnEvent += showStartBtn;
        gameManager.showRestartBtnEvent += showRestartBtn;
        gameManager.sentProgress += setValueOfProgressBar;
        gameManager.sentResult += setTxt;

        Canvas.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void showStartBtn()
    {
        Start_Btn.SetActive(true);
    }

    void showRestartBtn() {
        Restart_Btn.SetActive(true);
    }

    public void onClickStart() {
        StartEvent?.Invoke();
        Start_Btn.SetActive(false);
        showProgressSlider(true);
    }

    public void onClickRestart()
    {
        RestartEvent?.Invoke();
        Restart_Btn.SetActive(false);
        showProgressSlider(false);
        showTxt(false);
    }

    void showProgressSlider(bool isOn) {
        Progress_Slider.SetActive(isOn);
    }

    void setValueOfProgressBar(float x) {
        Progress_Slider.GetComponent<Slider>().value = x;
    }


    void showTxt(bool isOn)
    {
        resutlTxt.SetActive(isOn);
    }

    void setTxt(bool isOn, string txt, Color color) {
        showTxt(isOn);
        resutlTxt.GetComponent<TMP_Text>().text = txt;
        resutlTxt.GetComponent<TMP_Text>().color = color;
    }
}
