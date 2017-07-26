using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using HoloToolkit.Unity;
namespace YiHe {
    /// <summary>
    /// 产生统一移动的按钮。
    /// </summary>
    /// <seealso cref="GDGeek.Singleton{YiHe.MoveAll}" />
    /// <seealso cref="HoloToolkit.Unity.InputModule.IInputClickHandler" />
    public class MoveAll : GDGeek.Singleton<MoveAll>, HoloToolkit.Unity.InputModule.IInputClickHandler
    {
        /// <summary>
        /// gizmo原型
        /// </summary>
        public Gizmo _phototype = null;
    
        private Gizmo gizmo_ = null;
        /// <summary>
        /// 显示按键，
        /// </summary>
        public void show() {

            this.gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏按键.
        /// </summary>
        public void hide() {
            this.gameObject.SetActive(false);
        }
        /// <summary>
        /// 当用户点击的时候调用
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (gizmo_ != null) {
                GameObject.Destroy(gizmo_.gameObject);
            }

            gizmo_ = GameObject.Instantiate(_phototype);
            gizmo_.gameObject.SetActive(true);
            TapToPlaceOnce once = gizmo_.gameObject.GetComponent<TapToPlaceOnce>();
            if (once == null) {
                once = gizmo_.gameObject.AddComponent<TapToPlaceOnce>();
            }
            once.onBegin += delegate
            {
                gizmo_.unlock();
             
            };
            once.onEnd += delegate
            {
                gizmo_.lockIt();
            };
           
        }
        
    }
}
