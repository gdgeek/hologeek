using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoloGeek { 
    namespace Net { 
        public class MessageReader :  IMessageReader
        {
            public Action<NetworkInMessage> onReadFrom;

            public void readFrom(NetworkInMessage msg)
            {
                HoloDebug.Log("readFrom");
                if (onReadFrom != null)
                {
                    onReadFrom(msg);
                }
            }


        }
    }
}