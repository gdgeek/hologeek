using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        /// <summary>
        /// 共享管理，采用了自动列表模式来管理所有的共享（share）。
        /// </summary>
        /// <seealso cref="GDGeek.Singleton{HoloGeek.SynchroManager}" />
        public class SynchroManager : GDGeek.Singleton<SynchroManager>, IShareData, ISynchroData {

            public class SynchroManagerWriter : IMessageWriter
            {
                private Dictionary<string, IMessageWriter> map_ = new Dictionary<string, IMessageWriter>();
                public void add(string shareId, IMessageWriter writer) {
                    map_[shareId] = writer;
                }
                public void writeTo(NetworkOutMessage msg)
                {

                    msg.Write((int)map_.Count);
                    foreach (var kv in map_) {
                        msg.Write(new XString(kv.Key));
                        kv.Value.writeTo(msg);
                    }
                }
            }

            public class SynchroManagerReader : IMessageReader
            {

                private SynchroManager manager_ = null;
                public SynchroManagerReader(SynchroManager manager) {
                    manager_ = manager;
                }
       
                public void readFrom(NetworkInMessage msg)
                {
                    int count = msg.ReadInt32();
                    for (int i = 0; i < count; ++i)
                    {
                        XString shareId = msg.ReadString();


                        ISynchro synchro = manager_.getSynchro(shareId.GetString());

                        if (synchro != null)
                        {
                            IMessageReader reader = synchro.getDirtyReader();
                            reader.readFrom(msg);
                        }
                        else
                        {
                            Debug.LogError("no share" + shareId.GetString());
                        }
                    }
                }
            }

      
            public ISynchro getSynchro(string shareId)
            {
                if (map_.ContainsKey(shareId)) {
                    return map_[shareId];
                }
                return null;
            }

            private Dictionary<string, ISynchro> map_ = new Dictionary<string, ISynchro>();
            public void add(ISynchro synchro) {
                map_.Add(synchro.shareId, synchro);
            }
            public void remove(ISynchro synchro) {
                map_.Remove(synchro.shareId);
            }
            public bool dirty() {
                foreach (var kv in map_)
                {
                    ISynchro synchro = kv.Value;

                    if (synchro != null && synchro.dirty())
                    {
                        return true;
                    }

                }
                return false;
            }
            public void sweep() {
                foreach (var kv in map_)
                {
                    ISynchro synchro = kv.Value;

                    if (synchro != null && synchro.dirty())
                    {
                        synchro.sweep();
                    }


                }
            }
            public IMessageWriter getWriter() {

                SynchroManagerWriter writer = null;
                foreach (var kv in map_)
                {
                    ISynchro synchro = kv.Value;

                    if (synchro != null && synchro.dirty())
                    {
                        IMessageWriter sw = synchro.getDirtyWriter();
                        if (sw != null)
                        {
                            if (writer == null)
                            {
                                writer = new SynchroManagerWriter();
                            }

                            writer.add(synchro.shareId, sw);
                        }
                    }


                }
                return writer;
            }

            //public void pointer(Share share, )
            public void Update() {
                if (!HoloHelper.HasServer()) {
                    return;
                }
                SynchroManagerWriter writer = null;
                foreach (var kv in map_) {
                    ISynchro synchro = kv.Value;

                    if (synchro != null && synchro.dirty())
                    {

                        IMessageWriter sw = synchro.getDirtyWriter();
                        if (sw != null)
                        {
                            if (writer == null)
                            {
                                writer = new SynchroManagerWriter();
                            }

                            writer.add(synchro.shareId, sw);
                        }
                        synchro.sweep();
                    }

                
                }
                if (writer != null) {
                    if (ServerHandler.Instance.server != null) {
                        ServerHandler.Instance.server.synchro(writer);
                    }
                }

            }
      
            public IMessageReader getReader() {

                SynchroManagerReader reader = new SynchroManagerReader(this);
                return reader;
           
            }
        }
    }
}