using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    [RequireComponent(typeof(ShareId))]
    public class ShareDestory : MonoBehaviour {

        private ShareId shareId_ = null;
        public string shareId
        {
            get
            {
                if (shareId_ == null)
                {
                    shareId_ = this.gameObject.GetComponent<ShareId>();
                }
                return shareId_._shareId;
            }
        }
        private void OnDestroy()
        {
            YiHe.SampleFunctor.Instance.destoryObj(this.shareId);
           // FunctorManager.Instance.shareDestory(this.shareId);
        }
    }
}