using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        [RequireComponent(typeof(Synchro))]
        public class OwnerPointer : SynchroData {
            public Action onRefresh;
            private int ownerUserId_ = -1;
            private int lastOwnerUserId = -1;
      
            public void occupy() {
                ownerUserId_ = SharingStage.Instance.Manager.GetLocalUser().GetID();
            }
            public override bool dirty()
            {
                return (lastOwnerUserId != ownerUserId_);
            }
            public bool isOwner(){
                if (HoloHelper.HasServer()) { 
                    return ownerUserId_ == SharingStage.Instance.Manager.GetLocalUser().GetID();
                }
                return true;
            }
            public override void sweep()
            {
                if (ownerUserId_ != lastOwnerUserId)
                {
                    lastOwnerUserId = ownerUserId_;

                    if (onRefresh != null)
                    {
                        onRefresh();
                    }
            
                }
            }

            public override IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    ownerUserId_ =  msg.ReadInt32();
                };
                return reader;
            }

            public override IMessageWriter getWriter()
            {
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    msg.Write((int)ownerUserId_);
                };
                return writer;
            }
        }
    }

}