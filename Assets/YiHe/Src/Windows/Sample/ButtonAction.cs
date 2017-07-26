using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using GDGeek;
namespace YiHe {
    /// <summary>
    /// 基本按键动作
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="HoloToolkit.Unity.InputModule.IInputClickHandler" />
    /// <seealso cref="HoloToolkit.Unity.InputModule.IFocusable" />
    /// <seealso cref="HoloToolkit.Unity.InputModule.IInputHandler" />
    public class ButtonAction : MonoBehaviour,  HoloToolkit.Unity.InputModule.IFocusable, HoloToolkit.Unity.InputModule.IInputHandler
    {
        public GameObject _offset;
  
        public void OnFocusEnter()
        {
      
        }

        public void OnFocusExit()
        {
            TweenLocalPosition.Begin(_offset.gameObject, 0f, new Vector3(0, 0, 0));
        }

      
        public void OnInputDown(InputEventData eventData)
        {
            TweenLocalPosition.Begin(_offset.gameObject, 0.1f, new Vector3(0, 0, 0.01f));
        }

        public void OnInputUp(InputEventData eventData)
        {
            TweenLocalPosition.Begin(_offset.gameObject, 0f, new Vector3(0, 0, 0));
        }

    }

}