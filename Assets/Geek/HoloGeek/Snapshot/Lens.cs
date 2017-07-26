using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    namespace Snapshot {
        /// <summary>
        /// 将目标对象保存为json以及反序列化。
        /// </summary>
        public class Lens
        {
            [Serializable]
            public class Photo
            {
                [SerializeField]
                public PhotoTransform transform;
                [SerializeField]
                public string type;
                [SerializeField]
                public string json;
                // public 
            }
            [Serializable]
            public class Photos {
                [SerializeField]
                public Photo[] list = null;
            }
            static public string TakePhoto() {
                Target[] targets= Root.Instance.GetComponentsInChildren<Target>();
            
                Photos photos = new Photos();
                photos.list = new Photo[targets.Length];
                for (int i = 0; i < targets.Length; ++i)
                {

                    Target target = targets[i];
                    Photo photo = new Photo();
                    photo.type = target.type;
                    photo.json = FactoriesManager.Instance.serialize(target);
                    photo.transform = new PhotoTransform(target.transform);
                    photos.list[i] = photo;
                  
                }
                return JsonUtility.ToJson(photos);
            }


            internal static Target Create(string type, string json, PhotoTransform pt)
            {

                Target target = FactoriesManager.Instance.unserialize(type, json);
                pt.readTo(Root.Instance.transform, target.transform);
                return target;
            }
            internal static Target Create(string type, Target.IParameter data, PhotoTransform pt)
            {
                return Create(type, data.toJson(), pt);
            }

            static public Target[] DevelopPhoto(string json) {
                Photos photos = JsonUtility.FromJson<Photos>(json);

                Target[] targets = new Target[photos.list.Length];
                for (int i = 0; i < photos.list.Length; ++i) {
                    Photo photo = photos.list[i];
                    targets[i] = Create(photo.type, photo.json, photo.transform);
                }
                return targets;
            }
        }
    }
}
