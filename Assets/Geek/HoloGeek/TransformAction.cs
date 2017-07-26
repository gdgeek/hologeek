using UnityEngine;
namespace HoloGeek
{
    public class TransformAction : IAction
    {
        private int id_;
        private Vector3 position_;
        private Quaternion rotation_;
        private Vector3 scale_;
        public TransformAction(int id, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.id_ = id;
            this.position_ = position;
            this.rotation_ = rotation;
            this.scale_ = scale;
        }
        public Vector3 scale
        {
            get
            {
                return scale_;
            }

            set
            {
                scale_ = value;
            }
        }
        public Quaternion rotation
        {
            get
            {
                return rotation_;
            }

            set
            {
                rotation_ = value;
            }
        }
        public int id
        {
            get
            {
                return id_;
            }

            set
            {
                id_ = value;
            }
        }
        public Vector3 position
        {
            get
            {
                return position_;
            }

            set
            {
                position_ = value;
            }
        }

        public void execute(HoloToolkit.Sharing.User sender)
        {
            if(sender != HoloHelper.LocalUser()) { 
                ResourceManagber.Instance.holoTransform(id, position, rotation, scale);
            }
        }
    }
}