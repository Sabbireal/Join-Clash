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

    public event Action StartEvent;
    public event Action RestartEvent;

    public static bool loadEvents = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("UI Manager Loaded");
        subscribe();
        DontDestroyOnLoad(this.gameObject);
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
        FindObjectOfType<GameManager>().showStartBtnEvent += showStartBtn;
        FindObjectOfType<GameManager>().showRestartBtnEvent += showRestartBtn;
        FindObjectOfType<GameManager>().sentProgress += setValueOfProgressBar;
        FindObjectOfType<GameManager>().sentResult += setTxt;
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
