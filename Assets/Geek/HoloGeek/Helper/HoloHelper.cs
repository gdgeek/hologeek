using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    public class HoloHelper {

        public static bool IsEditor() {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
        public static bool HasEditor() {
            if (EditorUser() != null) {
                return true;
            }
            return false;

        }
        public static HoloToolkit.Sharing.User EditorUser() {
           
            if (SpectatorView.HolographicCameraManager.Instance != null)
            {
                return SpectatorView.HolographicCameraManager.Instance.editorUser;
            }
            return null;
        }
        public static bool HasServer() {
            HoloToolkit.Sharing.SharingStage sharingStage = HoloToolkit.Sharing.SharingStage.Instance;

            if (sharingStage == null || sharingStage.Manager == null)
            {
                //Debug.Log("Cannot Initialize HasServer. No SharingStage instance found.");
                return false;
            }
            HoloToolkit.Sharing.NetworkConnection serverConnection  = sharingStage.Manager.GetServerConnection();
           
            //serverConnection.IsConnected
            if (serverConnection == null)
            {
                Debug.Log("Cannot initialize HasServer. Cannot get a server connection.");
                return false;
            }
            return serverConnection.IsConnected();
            //return true;
        }
        public static bool IsSpectatorView() {
            if (SpectatorView.HolographicCameraManager.Instance != null && SpectatorView.HolographicCameraManager.Instance.localIPs.Contains(SpectatorView.HolographicCameraManager.Instance.HolographicCameraIP.Trim())) {
                return true;
            }
            return false;
        }

        public static bool HasSpectatorView() {
            if (SpectatorViewUser() != null)
            {
                return true;
            }
            return false;
        }
        public static HoloToolkit.Sharing.User SpectatorViewUser()
        {
            if (SpectatorView.HolographicCameraManager.Instance != null)
            {
                return SpectatorView.HolographicCameraManager.Instance.tppcUser;
            }
            return null;
        }

        public static HoloToolkit.Sharing.User LocalUser() {
            return HoloToolkit.Sharing.SharingStage.Instance.Manager.GetLocalUser();
        }

        public static bool IsIdLowest() {
            long localUserId;
            using (HoloToolkit.Sharing.User localUser = HoloToolkit.Sharing.SharingStage.Instance.Manager.GetLocalUser()) {
                localUserId = localUser.GetID();
               // HoloDebug.Log("user id" + localUserId +"!"+ HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers.Count);
            }
            for (int i = 0; i < HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers.Count; ++i) {
                if (HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[i].GetID() < localUserId) {

                       return false;
                }
              
            }
            return true;
        }
        /*
        public static HoloToolkit.Sharing.User IdLowestUser() {

            if (HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers.Count <= 0) {
                return null;
            }
            long lId = HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[0].GetID();
            int idx = 0;
            for (int i = 1; i < HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers.Count; ++i)
            {
                if (HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[i].GetID() < lId)
                {
                    lId = HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[i].GetID();
                    idx = i;
                }
            }
            return HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[idx];

        }
        */
        public static bool IsRoomCreater() {
            if (HasEditor())
            {
                if (IsSpectatorView())
                {
                    return true;
                }
            }
            else if (IsHost()) {
                return true;
            }
            return false;
        }
        public static bool IsHost() {

            if (!HasServer())
            {
              //  HoloDebug.Log("no server");
                return true;
            }

            /*
            if (IsEditor())
            {
                HoloDebug.Log("is editor");
                return true;
            }

            if (HasEditor())
            {
                HoloDebug.Log("has editor");
                return false;
            }*/
            if (LocalUser().GetID() == HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[0].GetID()) {
                return true;
            }
            return false;
            /*
            if (IsIdLowest())
            {
                HoloDebug.Log("IsIdLowest");
                return true;
            }
           
            return false;
            */
        }

        public static HoloToolkit.Sharing.User HostUser()
        {

            return HoloToolkit.Sharing.SharingStage.Instance.SessionUsersTracker.CurrentUsers[0];
            /* if (!HasServer())
             {
                 return LocalUser();
             }
             if (IsEditor())
             {
                 return LocalUser();
             }

             if (HasEditor())
             {
                 return EditorUser();
             }

             if (IsIdLowest())
             {
                 return LocalUser();
             }
             return IdLowestUser();
             */
        }

    }
}