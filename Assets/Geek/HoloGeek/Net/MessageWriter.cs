using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek { 
    public class MessageWriter: IMessageWriter
    {
        public Action<NetworkOutMessage> onWriteTo;

        public void writeTo(NetworkOutMessage msg)
        {
            if (onWriteTo != null)
            {
                onWriteTo(msg);
            }
        }


    }
}