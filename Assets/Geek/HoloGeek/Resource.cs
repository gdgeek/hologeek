using HoloGeek.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public class Resource : MonoBehaviour
    {
        
        private Vector3 lastPosition;
        private Quaternion lastRotation;
        private Vector3 lastScale;
        public int _id;
        private HoloGeek.Net.GeekMessages customMessages = null;

       
        void Start()
        {
           
           // guid = Guid.NewGuid().ToString();
            customMessages = HoloGeek.Net.GeekMessages.Instance;
            if (customMessages != null)
            {
                RegisterCustomMessages();
            }

            lastPosition = transform.localPosition;
            lastRotation = transform.localRotation;
            lastScale = transform.localScale;

           // Debug.Log();
        }
       
        void RegisterCustomMessages()
        {
                customMessages.MessageHandlers[GeekMessages.GeekMessageID.ServerTransform] = OnHoloTransform;
        }
        void serverTransform(HoloToolkit.Sharing.User sender, Vector3 lastPosition, Quaternion lastRotation, Vector3 lastScale) {
            if (HoloHelper.HasServer() && HoloHelper.LocalUser() != sender) {
                lastPosition = transform.localPosition = lastPosition;
                lastRotation = transform.localRotation = lastRotation;
                lastScale = transform.localScale = lastScale;
            }
        }
        void OnHoloTransform(HoloToolkit.Sharing.NetworkInMessage msg)
        {
            msg.ReadInt64();

            transform.localPosition = GeekMessages.Instance.ReadVector3(msg);
            transform.localRotation = GeekMessages.Instance.ReadQuaternion(msg);
            transform.localScale = GeekMessages.Instance.ReadVector3(msg);
        }

        void Update()
        {

            
            if (customMessages == null)
            {
                customMessages = GeekMessages.Instance;
                if (customMessages != null)
                {
                    RegisterCustomMessages();
                }
            }

            if (customMessages == null)
            {
                return;
            }

            if (!lastPosition.Equals(transform.localPosition) || !lastRotation.Equals(transform.localRotation) || !lastScale.Equals(transform.localScale))
            {
                //customMessages.SendHoloTransform(transform.localPosition, transform.localRotation, transform.localScale);
                TransformAction action = new TransformAction(_id, transform.localPosition, transform.localRotation, transform.localScale);
               
             //   ServerHandler.Instance.excute(action);
                lastPosition = transform.localPosition;
                lastRotation = transform.localRotation;
                lastScale = transform.localScale;
            }
        }
       

         
    }

}