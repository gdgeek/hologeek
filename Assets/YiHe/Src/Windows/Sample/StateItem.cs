using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public abstract class StateItem : MonoBehaviour
    {
        private bool pressed_ = false;
        protected BoxCollider objBoxCollider_;

        protected virtual void Awake()
        {
            objBoxCollider_ = GetComponent<BoxCollider>();
        }
        protected abstract void press();
        protected abstract void unpress();


        public bool pressed
        {
            get {
                return pressed_;
            }
            set {
                pressed_ = value;
            }
        }

        public void changeState()
        {
            pressed_ = !pressed_;
            if (pressed_)
            {
                press();
            }
            else
            {
                unpress();
            }
        }
    }
}