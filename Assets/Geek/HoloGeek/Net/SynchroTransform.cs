using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Sharing;
using UnityEngine;
namespace HoloGeek {
    namespace Net
    {
        /// <summary>
        /// 同步Tranform  
        /// </summary>
        /// <seealso cref="HoloGeek.ShareData" />

        [RequireComponent(typeof(Synchro))]
        public class SynchroTransform : SynchroData
        {

            public class Data
            {
                [SerializeField]
                public Vector3 localPosition;
                [SerializeField]
                public Quaternion localRotation;
                [SerializeField]
                public Vector3 localScale;
            }
            private Data data_ = new Data();

            private Data target_ = new Data();

            public class SynchroTransformHandler : IMessageReader, IMessageWriter
            {

                private SynchroTransform st_ = null;
                public SynchroTransformHandler(SynchroTransform st)
                {
                    st_ = st;
                }
                public void readFrom(NetworkInMessage msg)
                {
                    st_.transform.localPosition = MessageHalper.ReadVector3(msg);
                    st_.transform.localRotation = MessageHalper.ReadQuaternion(msg);
                    st_.transform.localScale = MessageHalper.ReadVector3(msg);
                }

                public void writeTo(NetworkOutMessage msg)
                {

                    MessageHalper.AppendVector3(msg, st_.transform.localPosition);
                    MessageHalper.AppendQuaternion(msg, st_.transform.localRotation);
                    MessageHalper.AppendVector3(msg, st_.transform.localScale);
                }
            }




            public override bool dirty()
            {
                if (data_.localPosition != this.transform.localPosition ||
                    data_.localRotation != this.transform.localRotation ||
                    data_.localScale != this.transform.localScale)
                {
                    return true;
                }
                return false;
            }




            public override void sweep()
            {
                data_.localPosition = this.transform.localPosition;
                data_.localRotation = this.transform.localRotation;
                data_.localScale = this.transform.localScale;
            }

            public override IMessageWriter getWriter()
            {

                MessageWriter writer = new MessageWriter();
                writer.onWriteTo += delegate (NetworkOutMessage msg)
                {
                    MessageHalper.AppendVector3(msg, this.transform.localPosition);
                    MessageHalper.AppendQuaternion(msg, this.transform.localRotation);
                    MessageHalper.AppendVector3(msg, this.transform.localScale);
                };

                return writer;
            }

            public override IMessageReader getReader()
            {

                MessageReader reader = new MessageReader();
                reader.onReadFrom += delegate (NetworkInMessage msg)
                {
                    this.transform.localPosition = MessageHalper.ReadVector3(msg);
                    this.transform.localRotation = MessageHalper.ReadQuaternion(msg);
                    this.transform.localScale = MessageHalper.ReadVector3(msg);
                };

                return reader;


            }
        }
    }
}