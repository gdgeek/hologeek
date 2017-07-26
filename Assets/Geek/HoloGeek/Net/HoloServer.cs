
using HoloToolkit.Sharing;
namespace HoloGeek {
    namespace Net { 
        public class HoloServer : Server {

            public override void mirrorTo(User user)
            {
                HoloDebug.Log("mirror to!");
                NetworkOutMessage oMsg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.Mirror);
                var writer = ShareManager.Instance.getWriter();
                writer.writeTo(oMsg);
                GeekMessages.Instance.sendToUser(user, oMsg);
            }


            public void Start()
            {
                //GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.HostSynchro] = onHostSynchro;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.HostFunctor] = onHostFunctor;
                GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.NeedMirror] = onNeedMirror;
            }

            private void onNeedMirror(NetworkInMessage msg)
            {
                long userId = msg.ReadInt64();


                NetworkOutMessage oMsg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.Mirror);
                var writer = ShareManager.Instance.getWriter();
                writer.writeTo(oMsg);
                User user = SharingStage.Instance.SessionUsersTracker.GetUserById((int)userId);
                GeekMessages.Instance.sendToUser(user, oMsg);

            }

            public void OnDestroy()
            {
                if (GeekMessages.HasInstance)
                {
                   // GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.HostSynchro] = null;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.HostFunctor] = null;
                    GeekMessages.Instance.MessageHandlers[GeekMessages.GeekMessageID.NeedMirror] = null;
                }
            }
            public void onHostFunctor(HoloToolkit.Sharing.NetworkInMessage iMsg)
            {
                long userId = iMsg.ReadInt64();
                Functor.Handler handler = new Functor.Handler();

                handler.readFrom(iMsg);


                NetworkOutMessage oMsg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.BroadcastFunctor);

                oMsg.Write(userId);
                handler.writeTo(oMsg);


                FunctorManager.Instance.functor(handler);
                GeekMessages.Instance.broadcast(oMsg);
            }
           /* public void onHostSynchro(HoloToolkit.Sharing.NetworkInMessage iMsg)
            {
                long userId = iMsg.ReadInt64();
                IMessageReader reader = SynchroManager.Instance.getReader();
                reader.readFrom(iMsg);



                NetworkOutMessage oMsg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.BroadcastSynchro);
                oMsg.Write(userId);
                var writer = SynchroManager.Instance.getWriter();
                writer.writeTo(oMsg);
                SynchroManager.Instance.sweep();
                GeekMessages.Instance.broadcast(oMsg);
            }

    */
            public override void functor(Functor.Handler handler)
            {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.BroadcastFunctor);
                msg.Write(GeekMessages.Instance.localUserID);
                handler.writeTo(msg);
                GeekMessages.Instance.broadcast(msg);
            }
          /*  public override void synchro(IMessageWriter writer)
            {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.BroadcastSynchro);
                msg.Write(GeekMessages.Instance.localUserID);

                writer.writeTo(msg);
                GeekMessages.Instance.broadcast(msg);
            }
            */
            public override void createObj(IMessageWriter writer)
            {
                NetworkOutMessage msg = GeekMessages.Instance.createMessage((byte)GeekMessages.GeekMessageID.ObjCreater);
                msg.Write(GeekMessages.Instance.localUserID);
                writer.writeTo(msg);
                GeekMessages.Instance.broadcast(msg);
            }

            public override void destoryObj(string shareId)
            {

            }
        }
    }
}
 