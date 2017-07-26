using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;

namespace HoloGeek
{
    namespace Net { 
        /// <summary>
        /// 同步数据用的
        /// </summary>
        /// <seealso cref="UnityEngine.MonoBehaviour" />
        /// Share
        /// 
        [RequireComponent(typeof(ShareId))]
        public class Share : MonoBehaviour, IShare
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

            public IMessageReader getReader()
            {
                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate(NetworkInMessage msg)
                {
                    for (int i = 0; i < _datas.Length; ++i) {
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
            public ShareData[] _datas;

     




        }
    }
}
