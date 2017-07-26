using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    namespace Net { 
        public class ServerLens : ShareData {
            public override IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    XString json = msg.ReadString();
                    Snapshot.Lens.DevelopPhoto(json.GetString());
                };
                return reader;
            }

            public override IMessageWriter getWriter()
            {
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    msg.Write(new XString(Snapshot.Lens.TakePhoto()));
                };
                return writer;
            }

      
        }
    }
}