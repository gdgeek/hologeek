using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek {

    /// <summary>
    /// 用来把接口转换成函数的辅助类型，便于实现从网络同步数据
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="HoloGeek.IMessageReader" />
    /// <seealso cref="HoloGeek.IMessageReader" />
    public class MessageHandler : MonoBehaviour, IMessageReader, IMessageWriter
    {

        public Action<NetworkInMessage> onReadFrom;
        public Action<NetworkOutMessage> onWriteTo;
        public void readFrom(NetworkInMessage msg)
        {
            if (onReadFrom == null) {
                onReadFrom(msg);
            }
        }

        public void writeTo(NetworkOutMessage msg)
        {
            if (onWriteTo == null)
            {
                onWriteTo(msg);
            }
        }

       
		
	}
	
	

}