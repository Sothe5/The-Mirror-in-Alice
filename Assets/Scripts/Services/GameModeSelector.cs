﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModeSelector : MonoBehaviour/*, ISelectHandler */{

    public Button vsModeButton, coopModeButton, endlessModeButton;
    public Image help;

	// Use this for initialization
	void Start () {
        Button btn1 = vsModeButton.GetComponent<Button>();
        Button btn2 = coopModeButton.GetComponent<Button>();
        Button btn3 = endlessModeButton.GetComponent<Button>();

        //OnClick listeners
        btn1.onClick.AddListener(vsModeClicked);
        btn2.onClick.AddListener(coopModeClicked);
        btn3.onClick.AddListener(endlessModeClicked);
	}

    void vsModeClicked()
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the vs mode!");
    }

    void coopModeClicked()
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the coop mode!");
    }

    void endlessModeClicked()
    {
        //Output this to console when the Button is clicked
        Debug.Log("You have clicked the endless mode!");
    }

    //public void OnSelect(BaseEventData baseEventData) {
    //    switch (baseEventData.selectedObject.name) {
    //        case "VsButton":
    //            help.material.color = Color.black;
    //            break;
    //        default:
    //            break;
    //    }

    //}
}