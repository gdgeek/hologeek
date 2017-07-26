using GDGeek;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YiHe
{
    /// <summary>
    /// 样品类型，里面控制载入动画，样品模型，和文字
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Sample : MonoBehaviour
    {



        [Serializable]
        public class Data
        {
            [SerializeField]
            public int id;
            [SerializeField]
            public string url;
            [SerializeField]
            public int version;
            [SerializeField]
            public Vector3 size;

            public float radius
            {
                get
                {
                    float max = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
                    return max / 2f;
                }
            }
        }

        public LoadingBox _loading = null;
        public SampleOffset _offset = null;
        public TextTip _textTip = null;
        Sample.Data data_ = new Sample.Data();
        internal Vector3 _firstPlacePos;
        internal Quaternion _firstPlaceRot;
        internal Vector3 _firstPlaceScale;

        public Sample.Data data
        {

            set
            {
                data_ = value;
            }
            get
            {
                return data_;
            }
        }

        public bool placed
        {
            get;
            set;
        }

        private void Awake()
        {
            _loading = GetComponentInChildren<LoadingBox>();
            _offset = GetComponentInChildren<SampleOffset>();
            _textTip = GetComponentInChildren<TextTip>();
        }
      
        void Start()
        {

            TaskList tl = new TaskList();

            SphereCollider sc = this.GetComponent<SphereCollider>();
            sc.radius = data_.radius;

            _loading.setRadius(data_.radius);
            _offset.setRadius(data_.radius);

            SampleLoader.LoadTask load = SampleLoader.Instance.load(data_.url, data_.version);


            TaskManager.PushBack(load, delegate
            {
                _offset.setObj(load._obj);
            });
            tl.push(load);
            TaskSet ts = new TaskSet();
            ts.push(_offset.show());
            ts.push(_loading.hide());
            ts.push(_textTip.loadingComplete(data_));
            _textTip._target = transform;
            tl.push(ts);

            TaskManager.Run(tl);

        }




        internal void reset()
        {
            transform.localPosition = _firstPlacePos;
            transform.localRotation = _firstPlaceRot;
            transform.localScale = _firstPlaceScale;
        }
    }

}