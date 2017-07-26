using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    namespace Net
    {
        [RequireComponent(typeof(ShareId))]
        public class ShareObjManager : GDGeek.Singleton<ShareObjManager>, IShare
        {

            public void Start()
            {

                ShareManager.Instance.add(this);

            }

            public void OnDestroy()
            {
                if (ShareManager.HasInstance)
                {
                    ShareManager.Instance.remove(this);
                }
            }


            public ShareObj[] _data;
            private ShareId shareId_ = null;
            public string shareId
            {
                get
                {
                    if (shareId_ == null)
                    {
                        shareId_ = this.gameObject.GetComponent<ShareId>();
                    }
                    return shareId_._shareId;
                }
            }

            private Dictionary<string, ShareObj> map_ = new Dictionary<string, ShareObj>();

    

            public void create(ShareObj obj) {
                obj.shareId = Guid.NewGuid().ToString();
                map_[obj.shareId] = obj;
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    msg.Write(new XString(obj.type));
                    msg.Write(new XString(obj.shareId));
                    var ow = obj.getWriter();
                    ow.writeTo(msg);
                };
                ServerHandler.Instance.server.createObj(writer);
            }
            

            public IMessageReader remoteCreate() {
                //NetworkInMessage msg
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    XString type = msg.ReadString();
                    XString shareId = msg.ReadString();
                    ShareObj obj = this.createImpl(type.ToString(), shareId.ToString());
                   
                    if (obj != null) {
                        obj.remote = true;
                        IMessageReader or = obj.getReader();
                        or.readFrom(msg);
                        map_[shareId.ToString()] = obj;
                    }
                };
                return reader;
            }

            private ShareObj createImpl(string type, string shareId)
            {
                for (int i = 0; i < _data.Length; ++i) {
                    if (_data[i].type == type) {
                        ShareObj obj =  Instantiate(_data[i]);
                        obj.shareId = shareId;
                        return obj;
                    }
                }
                return null;
            }

            public void remoteDestory(string shareId)
            {
                ShareObj obj = map_[shareId];
                map_.Remove(shareId);
                Destroy(obj);

            }
            public void destory(ShareObj obj)
            {

                ServerHandler.Instance.server.destoryObj(obj.shareId);
                map_.Remove(obj.shareId);
            }

            public  IMessageWriter getWriter()
            {
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    msg.Write((int)(map_.Count));
                    foreach (var kv in map_)
                    {
                        msg.Write(new XString(kv.Value.type));
                        msg.Write(new XString(kv.Key));
                        IMessageWriter ow = kv.Value.getWriter(); 
                        ow.writeTo(msg);
                    }
                };
                return writer;
            }

            public IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    int count = msg.ReadInt32();
                    for (int i=0; i<count; ++i)
                    {
                        XString type = msg.ReadString();
                        XString shareId = msg.ReadString();
                        var obj = this.createImpl(type.GetString(), shareId.GetString());
                        if(obj != null) {
                            obj.remote = true;
                            this.map_[shareId.GetString()] = obj;
                            var or = obj.getReader();
                            or.readFrom(msg);
                        }
                     
                    }
                   
                };
                return reader;
            }
        }
    }
}
