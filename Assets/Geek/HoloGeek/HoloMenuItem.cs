using UnityEngine;
using System.Collections;
using HoloGeek;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;
using GDGeek;
using UnityEngine.Events;

public class HoloMenuItem : MonoBehaviour, IInputClickHandler, IFocusable, IInputHandler
{
    internal string text;
    Text textUI;

    public UnityEvent _onClicked = new UnityEvent();
    internal int id;

    private void Start()
    {

    }

    public void OnInputUp(HoloToolkit.Unity.InputModule.InputEventData eventData)
    {


    }
    public void OnInputDown(HoloToolkit.Unity.InputModule.InputEventData eventData)
    {
    }
    public void OnInputClicked(HoloToolkit.Unity.InputModule.InputClickedEventData eventData)
    {
        _onClicked.Invoke();
    }

    public void OnFocusEnter()
    {

    }

    internal void init()
    {
        if (textUI == null)
        {
            textUI = GetComponentInChildren<Text>();
        }
        if (textUI && !textUI.isActiveAndEnabled)
        {
            gameObject.SetActive(true);
            textUI.enabled = true;
        }
            textUI.text = text;
    }

    public void OnFocusExit()
    {

    }

}
