using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        public class ShareManager : GDGeek.Singleton<ShareManager>, IShareData {
            public void add(IShare share)
            {
                map_.Add(share.shareId, share);
            }
            public void remove(IShare share)
            {
                map_.Remove(share.shareId);
            }

            private Dictionary<string, IShare> map_ = new Dictionary<string, IShare>();
            /// <summary>
            /// Gets the writer.
            /// </summary>
            /// <returns></returns>
            public IMessageWriter getWriter()
            {
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {

                    msg.Write((int)map_.Count);
                    foreach (var kv in map_)
                    {
                        msg.Write(new XString(kv.Key));
                        var wrt = kv.Value.getWriter();
                        wrt.writeTo(msg);
                    }
                };
                return writer;

            }

            public IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();

                HoloDebug.Log("getReader");
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    HoloDebug.Log("onReadFrom");
                    int count = msg.ReadInt32();
                    for (int i = 0; i < count; ++i)
                    {
                        XString key = msg.ReadString();
                        HoloDebug.Log(key.GetString());
                        var read = this.map_[key.GetString()].getReader();
                        read.readFrom(msg);

                    }
                };
                return reader;
            }




        }
    }
}