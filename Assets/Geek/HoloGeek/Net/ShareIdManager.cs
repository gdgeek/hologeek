using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public class ShareIdManager : GDGeek.Singleton<ShareIdManager> {
        private Dictionary<string, ShareId> map_ = new Dictionary<string, ShareId>();
        public void add(ShareId shareId)
        {
         
            map_.Add(shareId._shareId, shareId);
        }
        public void remove(ShareId shareId)
        {
            map_.Remove(shareId._shareId);
        }

        public GameObject GetObjById(string shareId) {
            return map_[shareId].gameObject;
        }
    }
}