using HoloToolkit.Sharing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public class MessageHalper : MonoBehaviour {
        #region HelperFunctionsForWriting

     

        public static void AppendVector3(NetworkOutMessage msg, Vector3 vector)
        {
            msg.Write(vector.x);
            msg.Write(vector.y);
            msg.Write(vector.z);
        }

        public static void AppendQuaternion(NetworkOutMessage msg, Quaternion rotation)
        {
            msg.Write(rotation.x);
            msg.Write(rotation.y);
            msg.Write(rotation.z);
            msg.Write(rotation.w);
        }

        #endregion HelperFunctionsForWriting

        #region HelperFunctionsForReading

        public static Vector3 ReadVector3(NetworkInMessage msg)
        {
            return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
        }

        public static Quaternion ReadQuaternion(NetworkInMessage msg)
        {
            return new Quaternion(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
        }

        #endregion HelperFunctionsForReading


    }
}
