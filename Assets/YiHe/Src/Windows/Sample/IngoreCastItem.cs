using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public class IngoreCastItem : StateItem {

        public GameObject realObjCollider;
        public GameObject CastIcon;
        public GameObject NoCastIcon;


        // Use this for initialization
        protected override void press()
        {
           // base.press();
            realObjCollider.gameObject.SetActive(false);
            CastIcon.SetActive(false);
            NoCastIcon.SetActive(true);
        }

        protected override void unpress()
        {
        
            realObjCollider.gameObject.SetActive(true);
            CastIcon.SetActive(true);
            NoCastIcon.SetActive(false);
        }
    }

}