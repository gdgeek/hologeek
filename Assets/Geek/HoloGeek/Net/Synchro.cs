using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;

namespace HoloGeek {
    namespace Net { 
        /// <summary>
        /// 同步数据用的
        /// </summary>
        /// <seealso cref="UnityEngine.MonoBehaviour" />
        /// Share
        /// 
        [RequireComponent(typeof(ShareId))]
        public class Synchro : MonoBehaviour, ISynchro, IShare
        {

            public bool _isShare;

            public void Start()
            {
                if (_isShare) { 
                    ShareManager.Instance.add(this);
                }
                SynchroManager.Instance.add(this);
            
            }

            public void OnDestroy()
            {
                if (SynchroManager.HasInstance) { 
                    SynchroManager.Instance.remove(this);
                }

                if (_isShare && ShareManager.HasInstance)
                {
                    ShareManager.Instance.remove(this);
                }
            }
      
            public class SynchroWriter : IMessageWriter
            {
                private Dictionary<int, IMessageWriter> map_ = new Dictionary<int, IMessageWriter>();
                public void add(int id, IMessageWriter writer)
                {
                    map_[id] = writer;
                }
                public void writeTo(NetworkOutMessage msg)
                {
                    msg.Write((int)map_.Count);
                    foreach (var kv in map_)
                    {
                        msg.Write((int)kv.Key);
                        kv.Value.writeTo(msg);
                    }
                }
            }

            public class SynchroReader : IMessageReader
            {
                private Synchro synchro_ = null;
                public SynchroReader(Synchro synchro)
                {
                    synchro_ = synchro;
                }
       

                public void readFrom(NetworkInMessage msg)
                {
                    int count = msg.ReadInt32();
                    for (int i = 0; i < count; ++i)
                    {

                        int id = msg.ReadInt32();

                        SynchroData data = synchro_.getData(id);


                        if (data != null)
                        {
                            IMessageReader reader = data.getReader();
                            reader.readFrom(msg);
                        }
                        else
                        {
                            Debug.LogError("no data" + data.ToString());
                        }
                    }
                }
            }


            private ShareId share_ = null;
            public string shareId
            {
                get
                {
                    if (share_ == null)
                    {
                        share_ = this.gameObject.GetComponent<ShareId>();
                    }
                    return share_._shareId;
                }
            }
            private SynchroData getData(int id)
            {
                return _datas[id];
            }

            public SynchroData[] _datas;
       
            public void sweep()
            {
                for (int i = 0; i < _datas.Length; ++i)
                {
                    if (_datas[i].dirty()) {
                        _datas[i].sweep();
                    }
                }
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

            public bool dirty()
            {
                for (int i = 0; i < _datas.Length; ++i)
                {
                    if (_datas[i].dirty())
                        return true;
                }

                return false;
            }

            public IMessageWriter getDirtyWriter()
            {
                SynchroWriter writer = null;
                for (int i = 0; i < _datas.Length; ++i)
                {
                    if (_datas[i].dirty())
                    {
                        if (writer == null)
                        {
                            writer = new SynchroWriter();
                        }
                        writer.add(i, _datas[i].getWriter());
                    }
                }

                return writer;
            }

            public IMessageReader getDirtyReader()
            {
                SynchroReader reader = new SynchroReader(this);
                return reader;
            }
        }
    }
}