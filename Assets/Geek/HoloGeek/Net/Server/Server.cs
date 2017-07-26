using System;
using System.Collections;
using System.Collections.Generic;
using HoloGeek;
using UnityEngine;
using HoloToolkit.Sharing;

namespace HoloGeek {
    namespace Net { 
        /// <summary>
        /// 服务器基类
        /// </summary>
        /// <seealso cref="UnityEngine.MonoBehaviour" />
        public abstract class Server : MonoBehaviour, IServer {


            protected virtual void Start()
            {
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.ObjCreater] += onObjCreater;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.ObjDestory] += onObjDestory;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.Synchro] += onSynchro;

                
            }

          

            protected virtual void OnDisable()
            {
                if (GeekMessages.HasInstance)
                {
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.ObjCreater] -= onObjCreater;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.ObjDestory] -= onObjDestory;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.Synchro] -= onSynchro;

                    
                }

            }

            private void onSynchro(NetworkInMessage msg)
            {
                long userId = msg.ReadInt64();
              

                if (userId != GeekMessages.Instance.localUserID)
                {
                    var reader = SynchroManager.Instance.getReader();
                    reader.readFrom(msg);
                    SynchroManager.Instance.sweep();
                }
            }


            //protected virtual void o

            private void onObjCreater(NetworkInMessage msg) {
                IMessageReader reader = ShareObjManager.Instance.remoteCreate();
                reader.readFrom(msg);
            }

            private void onObjDestory(NetworkInMessage msg)
            {
                XString shareId =  msg.ReadString();
                ShareObjManager.Instance.remoteDestory(shareId.GetString());
            }



            public virtual void synchro(IMessageWriter writer) {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.Synchro);
               // msg.Write(GeekMessages.Instance.localUserID);

                writer.writeTo(msg);
                GeekMessages.Instance.broadcast(msg);
            }
            public abstract void functor(Functor.Handler handler);
            public virtual void mirrorTo(HoloToolkit.Sharing.User user) {

            }

            public virtual void createObj(IMessageWriter writer) {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)(GeekMessages.GeekMessageID.ObjCreater));
                writer.writeTo(msg);
                GeekMessages.Instance.broadcast(msg, MessageReliability.ReliableOrdered);


            }
            public virtual void destoryObj(string shareId) {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)(GeekMessages.GeekMessageID.ObjDestory));
                msg.Write(new XString(shareId));
                GeekMessages.Instance.broadcast(msg, MessageReliability.ReliableOrdered);
            }
        }
    }
}