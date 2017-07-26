using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloGeek { 
    namespace Net { 
        public class ServerProxy : Server
        {
            protected override void Start()
            {
                base.Start();
                //GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.BroadcastSynchro] += onSynchro;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.BroadcastFunctor] += onPointer;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.Mirror] += onMirror;
            }
            protected override void OnDisable()
            {
                base.OnDisable();
                if (GeekMessages.HasInstance) { 
                    
                   // GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.BroadcastSynchro] -= onSynchro;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.BroadcastFunctor] -= onPointer;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.Mirror] -= onMirror;
                }
            }
            private void onMirror(NetworkInMessage msg)
            {
                HoloDebug.Log("onMirror");
                long userId = msg.ReadInt64();


                var reader = ShareManager.Instance.getReader();
                reader.readFrom(msg);
                SynchroManager.Instance.sweep();

         
            }

            void onPointer(HoloToolkit.Sharing.NetworkInMessage msg)
            {
                long userId = msg.ReadInt64();
                long fromUserId = msg.ReadInt64();
           
                if (fromUserId != GeekMessages.Instance.localUserID)
                {
                    var handler = new Functor.Handler();
                    handler.readFrom(msg);
                    FunctorManager.Instance.functor(handler);
           
                }
            }

            internal void needMirror()
            {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.NeedMirror);
                GeekMessages.Instance.sendToHost(msg);
            }

           /* void onSynchro(HoloToolkit.Sharing.NetworkInMessage msg)
            {
                long userId = msg.ReadInt64();
                long fromUserId = msg.ReadInt64();

                if (fromUserId != GeekMessages.Instance.localUserID)
                {
                    var reader = SynchroManager.Instance.getReader();
                    reader.readFrom(msg);
                    SynchroManager.Instance.sweep();
                }
            }
            public override void synchro(IMessageWriter writer)
            {
                GeekMessages.Instance.hostSynchro(writer);
            }*/
      
            public override void functor(Functor.Handler handler)
            {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.HostFunctor);
                msg.Write(GeekMessages.Instance.localUserID);
                handler.writeTo(msg);
                GeekMessages.Instance.sendToHost(msg);
            }

         
        }
    }
}