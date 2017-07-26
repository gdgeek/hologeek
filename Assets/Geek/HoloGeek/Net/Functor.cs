using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        [RequireComponent(typeof(ShareId))]
        public class Functor : MonoBehaviour, IFunctor//, IShareData
        {

            public class Func {
                public string name;
                public Action<string> handler;
            };
            private ShareId share_ = null;


            public string shareId{
                get {
                    if (share_ == null) {
                        share_ = this.gameObject.GetComponent<ShareId>();
                    }
                    return share_._shareId;
                }
            }

            public void Start()
            {
                 FunctorManager.Instance.add(this);
            }

            public void OnDestroy()
            {
                if (FunctorManager.HasInstance) { 
                    FunctorManager.Instance.remove(this);
                }
            }

        
            public class Handler : IMessageHandler
            {
                public string shareId;
                public string func;
                public string parameter;
               // public IMessageHandler handler;
                public void writeTo(NetworkOutMessage msg)
                {

                    msg.Write(new XString(shareId));
                    msg.Write(new XString(func));
                    msg.Write(new XString(parameter));
                }
                public void readFrom(NetworkInMessage msg)
                {
                    this.shareId = msg.ReadString().GetString();
                    this.func = msg.ReadString().GetString();
                    this.parameter = msg.ReadString().GetString();
                }
            }
            private Dictionary<string, Action<string> > actions_ = new Dictionary<string, Action<string> >();

        
            public void add(string func, Action<string> action) {
                actions_[func] = action;
            }
            public void execute(string func, string parameter)
            {
                Handler handler = new Handler();
                handler.shareId = this.shareId;
                handler.func = func;
                handler.parameter = parameter;
                if (ServerHandler.Instance.server != null)
                {
                    ServerHandler.Instance.server.functor(handler);
                }
            }
        
            public void callback(string func, string parameter) {
                if (this.actions_.ContainsKey(func)) {
                    this.actions_[func](parameter);
                }

            }
            /*
            public IMessageWriter getWriter()
            {
                throw new NotImplementedException();
            }

            public IMessageReader getReader()
            {
                throw new NotImplementedException();
            }
            */
            [Serializable]
            public class Int
            {
                [SerializeField]
                public int a;

                public Int(int a)
                {
                    this.a = a;
                }
            }
        }
    }
}