using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloGeek { 
    namespace Net { 
        public class ServerHandler : GDGeek.Singleton<ServerHandler>
        {

            private IServer server_ = null;
            private bool registeredSharingStageCallbacks;

            // private List<TransformAction> list_ = new List<TransformAction>();

            public IServer server { get { return server_; } }
     
     
            protected void OnDestroy()
            {
                registeredSharingStageCallbacks = false;

                if (SharingStage.Instance != null)
                {
                    SharingStage.Instance.SessionUsersTracker.UserJoined -= SessionUsersTracker_UserJoined;
                    SharingStage.Instance.SessionUsersTracker.UserLeft -= SessionUsersTracker_UserLeft;
                }

            }
      
            void refreshServer() {

                ServerProxy proxy = this.gameObject.GetComponent<ServerProxy>();
                ServerHost host = this.gameObject.GetComponent<ServerHost>();
                ServerNone none = this.gameObject.GetComponent<ServerNone>();
                if(!HoloHelper.HasServer()){
                    if(none == null) { 
                        none = this.gameObject.AddComponent<ServerNone>();
                    }
                    return;
                }


                if (none != null) {
                    Destroy(none);
                }

                if (HoloHelper.IsHost())
                {

                    HoloDebug.Log("Host");
                    if (proxy != null) {
                        Destroy(proxy);
                    }
                    if(host == null) {
                        host = this.gameObject.AddComponent<ServerHost>();
                    }
                    server_ = host;
                }
                else
                {

                    HoloDebug.Log("Proxy");
                    if (host != null)
                    {
                        Destroy(host);
                    }
                    if (proxy == null)
                    {
                        proxy = this.gameObject.AddComponent<ServerProxy>();
                        proxy.needMirror();
                    }
                    server_ = proxy;
                }
            }
            void Update () {

         
            
                if (!registeredSharingStageCallbacks && SharingStage.Instance != null && SharingStage.Instance.SessionUsersTracker != null)
                {
                    registeredSharingStageCallbacks = true;

                    SharingStage.Instance.SessionUsersTracker.UserJoined += SessionUsersTracker_UserJoined;
                    SharingStage.Instance.SessionUsersTracker.UserLeft += SessionUsersTracker_UserLeft;
                }


                if (server_ == null && ImportExportAnchorManager.Instance != null) {
                    if (ImportExportAnchorManager.Instance.currentState == ImportExportAnchorManager.ImportExportState.Ready)
                    {
                        refreshServer();
                    }
                }
	        }

            private void SessionUsersTracker_UserLeft(User obj)
            {
                refreshServer();
            }

            private void SessionUsersTracker_UserJoined(User obj)
            {
                refreshServer();
            }
        }
    }
}