using HoloToolkit.Sharing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        [RequireComponent(typeof(ShareId))]
        public class ShareObj : MonoBehaviour, IShareData{
            private bool remote_ = false;

            private ShareId shareId_ = null;
            public string type {

                get {
                    return "test";
                }
            }
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
                set {
                    if (shareId_ == null)
                    {

                        shareId_ = this.gameObject.GetComponent<ShareId>();
                    }
                    shareId_._shareId = value;
                }
            }

            public bool remote
            {
                get
                {
                    return remote_;
                }

                set
                {
                    remote_ = value;
                }
            }

            public IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                
                    for (int i = 0; i < _datas.Length; ++i)
                    {
                        _datas[i].getReader().readFrom(msg);
                    }
                };
                return reader;
            }

            public IMessageWriter getWriter()
            {
                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    for (int i = 0; i < _datas.Length; ++i)
                    {
                        _datas[i].getWriter().writeTo(msg);
                    }
                };

                return writer;
            }
            // Use this for initialization
            void Start () {
                if(remote_ == false) { 
                    ShareObjManager.Instance.create(this);
                }
                // ShareObjManager.Inst.create(this);

            }

            private void OnDestroy()
            {
                if (ShareObjManager.HasInstance) {
                    ShareObjManager.Instance.destory(this);
                }
               // ShareObjManager.Inst.destroy(this);
            }

            public ShareData[] _datas;
        }
    }
}