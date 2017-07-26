using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace YiHe {

    /// <summary>
    /// 用以注册网络函数。
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(HoloGeek.Net.Functor))]
    public class SampleFunctor : GDGeek.Singleton<SampleFunctor> {
        [Serializable]
        public class ObjCreaterParameter{
            [SerializeField]
            public string type;
            [SerializeField]
            public SampleTarget.Data data;
            [SerializeField]
            public Vector3 position;
            [SerializeField]
            public Quaternion rotation;
            [SerializeField]
            public Vector3 scale;
        }

        [Serializable]
        public class ObjDestoryParameter
        {
            [SerializeField]
            public string shareId;
          
        }

        internal void destoryObj(string shareId)
        {
           
        }

        // Use this for initialization
        void Start () {
            HoloGeek.Net.Functor functor = this.gameObject.GetComponent<HoloGeek.Net.Functor>();
            functor.add("obj.creater", delegate (string json)
            {
                ObjCreaterParameter parameter = JsonUtility.FromJson<ObjCreaterParameter>(json);
                createObjectImpl(parameter);

            });

            functor.add("obj.destory", delegate (string json)
            {
                ObjDestoryParameter parameter = JsonUtility.FromJson<ObjDestoryParameter>(json);
                destoryObjectImpl(parameter);

            });

        }

        private void destoryObjectImpl(ObjDestoryParameter parameter)
        {
            GameObject obj = HoloGeek.ShareIdManager.Instance.GetObjById(parameter.shareId);
            Destroy(obj);
        }

        public GameObject createObject(string type, SampleTarget.Data data, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            HoloGeek.Net.Functor functor = this.gameObject.GetComponent<HoloGeek.Net.Functor>();
            ObjCreaterParameter parameter = new ObjCreaterParameter();
            parameter.type = type;
            parameter.data = data;
            parameter.position = position;
            parameter.rotation = rotation;
            parameter.scale = scale;
            functor.execute("obj.creater", JsonUtility.ToJson(parameter));
            return createObjectImpl(parameter);
        }
        private GameObject createObjectImpl(ObjCreaterParameter parameter)
        {

            HoloGeek.Snapshot.Target target = HoloGeek.Snapshot.FactoriesManager.Instance.create(parameter.type, parameter.data);
            target.transform.position = parameter.position;
            target.transform.rotation = parameter.rotation;
            target.transform.localScale = parameter.scale;
            return target.gameObject;
        }
    }
}
