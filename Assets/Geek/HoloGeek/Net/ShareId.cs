using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGeek {

    [ExecuteInEditMode]
    public class ShareId : MonoBehaviour {
        public String _shareId;
        void Update()
        {
            if (string.IsNullOrEmpty(this._shareId))
            {
                this._shareId = System.Guid.NewGuid().ToString();

            }
        }
     

    }



}