using HoloToolkit.Sharing;
using SpectatorView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloGeek
{ 
    namespace Net { 
        public class GeekMessages : GDGeek.Singleton<GeekMessages> {
            /// <summary>
            /// Message enum containing our information bytes to share.
            /// The first message type has to start with UserMessageIDStart
            /// so as not to conflict with HoloToolkit internal messages.
            /// </summary>
            public enum GeekMessageID : byte
            {
                ServerTransform = MessageID.UserMessageIDStart,//位移
                ClientTransform,
               // HostSynchro,
                Synchro,
                HostFunctor,
                BroadcastFunctor,
                ObjCreater,
                ObjDestory,
                NeedMirror,
                Mirror,
                Max,
            }


            internal bool broadcast(NetworkOutMessage msg, MessageReliability reliability = MessageReliability.UnreliableSequenced)
            {

                HoloDebug.Log("broadcast to user:");
                if (serverConnection != null && serverConnection.IsConnected())
                {
                    this.serverConnection.Broadcast(
                      msg,
                      MessagePriority.Immediate,
                      reliability,
                      MessageChannel.Avatar);
                     return true;
                }
                return false;
            }
            public bool isConnected()
            {
                if (serverConnection != null && serverConnection.IsConnected())
                {
                    return true;
                }
                return false;
            }
            internal bool sendToHost(NetworkOutMessage msg)
            {
                HoloDebug.Log("send to host:" + msg.ToString());
                if (serverConnection != null && serverConnection.IsConnected())
                {

                    serverConnection.SendTo(
                     HoloHelper.HostUser(),
                     ClientRole.Primary,
                     msg,
                     MessagePriority.Immediate,
                     MessageReliability.UnreliableSequenced,
                     MessageChannel.Avatar);

                    return true;
                }
                return false;

            }

            internal bool sendToUser(User user, NetworkOutMessage msg)
            {

                HoloDebug.Log("send to user:" + user.GetID() + ":" + msg.ToString());
                if (serverConnection != null && serverConnection.IsConnected())
                {

                    serverConnection.SendTo(
                     user,
                     ClientRole.Primary,
                     msg,
                     MessagePriority.Immediate,
                     MessageReliability.UnreliableSequenced,
                     MessageChannel.Avatar);

                    return true;
                }
                return false;
            }



            public enum UserMessageChannels
            {
                Anchors = MessageChannel.UserMessageChannelStart,
            }

        

            [HideInInspector]
            public bool Initialized = false;
            private long localUserID_;
            /// <summary>
            /// Cache the local user's ID to use when sending messages
            /// </summary>
            public long localUserID

            {
                get {
                    return localUserID_;
                }
                set {
              
                    localUserID_ = value;
                }
            }

            public delegate void MessageCallback(NetworkInMessage msg);
            private Dictionary<GeekMessageID, MessageCallback> _MessageHandlers = new Dictionary<GeekMessageID, MessageCallback>();
            public Dictionary<GeekMessageID, MessageCallback> MessageHandlers
            {
                get
                {
                    return _MessageHandlers;
                }
            }

            /// <summary>
            /// Helper object that we use to route incoming message callbacks to the member
            /// functions of this class
            /// </summary>
            NetworkConnectionAdapter connectionAdapter;

            /// <summary>
            /// Cache the connection object for the sharing service
            /// </summary>
            NetworkConnection serverConnection;

            SharingStage sharingStage;

            void Start()
            {
                InitializeMessageHandlers();
            }

            public void OnDisable()
            {
                Initialized = false;
            }

            void InitializeMessageHandlers()
            {
                sharingStage = SharingStage.Instance;
                if (sharingStage == null || sharingStage.Manager == null)
                {
                    return;
                }

                serverConnection = sharingStage.Manager.GetServerConnection();
                connectionAdapter = new NetworkConnectionAdapter();

                connectionAdapter.MessageReceivedCallback += OnMessageReceived;

                // Cache the local user ID
                this.localUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();

                for (byte index = (byte)GeekMessageID.ServerTransform; index < (byte)GeekMessageID.Max; index++)
                {
                    if (MessageHandlers.ContainsKey((GeekMessageID)index) == false)
                    {
                        MessageHandlers.Add((GeekMessageID)index, null);
                    }

                    serverConnection.AddListener(index, connectionAdapter);
                }

                Initialized = true;
            }

            void Update()
            {
                if (sharingStage == null)
                {
                    InitializeMessageHandlers();
                }
            }

            public NetworkOutMessage createMessage(byte MessageType)
            {
                if(serverConnection != null) { 
                    NetworkOutMessage msg = serverConnection.CreateMessage(MessageType);
                    msg.Write(MessageType);
                    // Add the local userID so that the remote clients know whose message they are receiving
                    msg.Write(localUserID);
                    return msg;
                }
                return null;
            }
    

            public void SendServerTransform(int id, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                // If we are connected to a session, broadcast the scene info
                if (serverConnection != null && serverConnection.IsConnected())
                {
                    NetworkOutMessage msg = createMessage((byte)GeekMessageID.ServerTransform);
                    msg.Write(id);
                    AppendTransform(msg, position, rotation, scale);

                    serverConnection.SendTo(
                         HoloHelper.HostUser(),
                         ClientRole.Primary,
                         msg,
                         MessagePriority.Immediate,
                         MessageReliability.UnreliableSequenced,
                         MessageChannel.Avatar);
                }
            }

            public void SendClientTransform(int id, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                // If we are connected to a session, broadcast the scene info
                if (serverConnection != null && serverConnection.IsConnected())
                {
                    NetworkOutMessage msg = createMessage((byte)GeekMessageID.ClientTransform);
                    msg.Write(id);
                    AppendTransform(msg, position, rotation, scale);

                    this.serverConnection.Broadcast(
                     msg,
                     MessagePriority.Immediate,
                     MessageReliability.UnreliableSequenced,
                     MessageChannel.Avatar);
                }
            }

            protected void OnDestroy()
            {
                if (this.serverConnection != null)
                {
                    for (byte index = (byte)GeekMessageID.ServerTransform; index < (byte)GeekMessageID.Max; index++)
                    {
                        this.serverConnection.RemoveListener(index, this.connectionAdapter);
                    }
                    this.connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
                }

          
            }

            void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg)
            {
                byte messageType = msg.ReadByte();
                MessageCallback messageHandler = MessageHandlers[(GeekMessageID)messageType];
                if (messageHandler != null)
                {
                    messageHandler(msg);
                }
            }

            #region HelperFunctionsForWriting
            void AppendTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation, Vector3 scale)
            {
                AppendVector3(msg, position);
                AppendQuaternion(msg, rotation);
                AppendVector3(msg, scale);
            }
            void AppendTransform(NetworkOutMessage msg, Vector3 position, Quaternion rotation)
            {
                AppendVector3(msg, position);
                AppendQuaternion(msg, rotation);
            }

            void AppendVector3(NetworkOutMessage msg, Vector3 vector)
            {
                msg.Write(vector.x);
                msg.Write(vector.y);
                msg.Write(vector.z);
            }

            void AppendQuaternion(NetworkOutMessage msg, Quaternion rotation)
            {
                msg.Write(rotation.x);
                msg.Write(rotation.y);
                msg.Write(rotation.z);
                msg.Write(rotation.w);
            }

            #endregion HelperFunctionsForWriting

            #region HelperFunctionsForReading

            public Vector3 ReadVector3(NetworkInMessage msg)
            {
                return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
            }

            public Quaternion ReadQuaternion(NetworkInMessage msg)
            {
                return new Quaternion(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
            }

            #endregion HelperFunctionsForReading
        }
    }
}