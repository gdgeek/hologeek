using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe {
    /// <summary>
    /// 整体移动用的控制器
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Gizmo : MonoBehaviour {

        private bool locked_ = false;

        private Vector3 _oldPosition;
     
        public void unlock()
        {
            locked_ = false;
        }

        public void lockIt()
        {
            locked_ = true;
            _oldPosition = this.transform.position;
        }
        
        /// <summary>
        /// 更新控制所有场景内ObjectManager的位置，根据这个节点的移动来调整。
        /// </summary>
        public void FixedUpdate() {
            if (locked_ && _oldPosition != this.transform.position) {
                Sample[] objs = Component.FindObjectsOfType<Sample>();
                for (int i = 0; i < objs.Length; ++i) {
                    objs[i].transform.position += this.transform.position - _oldPosition;
                }
                _oldPosition = this.transform.position;
            }
        }
    }

}