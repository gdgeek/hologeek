using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public abstract class ShareData : MonoBehaviour, IShareData
    {


        public abstract IMessageReader getReader();
        public abstract IMessageWriter getWriter();
    }
}