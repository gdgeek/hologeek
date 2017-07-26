using System;
using System.Collections;
using System.Collections.Generic;
using GDGeek;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using HoloToolkit.Unity;

namespace YiHe {
    /// <summary>
    /// 打样物品按键功能
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="HoloToolkit.Unity.InputModule.IInputClickHandler" />
    public class SampleItem : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
    {
       
     
       // public Sample _phototype;
        private SampleData data_ = null;
        public enum Mode {
            Normal,
            Close,
            Loading,
            Open,
        }

        public Mode _mode = Mode.Normal;
        private Mode _oldMode = Mode.Normal;
        public Texture2D _loadingImg = null;
        public Texture2D _image = null;
        public GameObject _face = null;
        public Renderer _renderer = null;

        //private CloneObjInfoManager cloneObjsManager;

        public void Start()
        {
            
            doNormal();
        }
      

        internal Task closeing()
        {
            Task task = new Task();
          
            TaskManager.PushBack(task, delegate {
             
                this._mode = Mode.Close;
            });

            return task;
        }

        internal Task loading(SampleData data)
        {
            ImageLoader.LoadTask task = ImageLoader.Instance.load(data._imageUrl);
            TaskManager.PushFront(task, delegate {
                data_ = data;
                this._mode = Mode.Loading;
            });

            TaskManager.PushBack(task, delegate {
                _image = task._texture;
                this._mode = Mode.Open;
            });
           
            return task;
        }

        public void doNormal()
        {
            _face.SetActive(false);
            _renderer.gameObject.SetActive(false);
        }

        private void doClose()
        {
            _face.SetActive(false);
            _renderer.gameObject.SetActive(false);
        }
        private void doOpen()
        {
            _renderer.transform.localEulerAngles = Vector3.zero;
            _face.SetActive(true);
            _renderer.material.mainTexture = this._image;
            _renderer.gameObject.SetActive(true);
        }

        private void doLoading()
        {
            _face.SetActive(true);
            _renderer.material.mainTexture = _loadingImg;
            _renderer.gameObject.SetActive(true);
        }
     
        public void Update()
        {
            if (_mode != _oldMode) {
                _oldMode = _mode;
                refreshMode(_mode);
            }
            updateMode();
        }

        private void updateMode()
        {
            if (_mode == Mode.Loading) {
                _renderer.transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 360f));
            }
        }

        private void refreshMode(Mode mode)
        {
            switch (mode) {
                case Mode.Normal:
                    doNormal();
                    break;
                case Mode.Close:
                    doClose();
                    break;
                case Mode.Loading:
                    doLoading();
                    break;
                case Mode.Open:
                    doOpen();
                    break;
            }
            
        }
      
        public virtual void OnInputClicked(InputClickedEventData eventData)
        {
            if (this._mode == Mode.Close) {
                return;
            }
            SampleTarget.Data data = this.createSampleData();
            GameObject obj = SampleFunctor.Instance.createObject("YiHeObj", data, this.transform.position, Quaternion.Euler(0, 0, 0), Vector3.one);// createSampleImpl("YiHeObj", data, this.transform.position, Quaternion.Euler(0,0,0), Vector3.one);
            tapToPlaceOnce(obj.GetComponent<Sample>());
        }
      
       
        private void tapToPlaceOnce(Sample sample)
        {
            TapToPlaceOnce once = sample.gameObject.AddComponent<TapToPlaceOnce>();
            HoloGeek.Snapshot.Target target = sample.GetComponent<HoloGeek.Snapshot.Target>();
            once.onBegin += delegate
            {
                sample.placed = false;
                Billboard bb = sample._offset.gameObject.AddComponent<Billboard>();
                bb.PivotAxis = PivotAxis.Y;
                target.enabled = false;
            };
            once.onEnd += delegate
            {
                sample.placed = true;
                sample._firstPlacePos = sample.transform.localPosition;
                sample._firstPlaceRot = sample.transform.localRotation;
                sample._firstPlaceScale = sample.transform.localScale;
                target.enabled = true;
                Billboard bb = sample._offset.gameObject.GetComponent<Billboard>();
                if (bb != null)
                {
                    Destroy(bb);
                }
            };
          
        }

        private SampleTarget.Data createSampleData()
        {
            SampleTarget.Data data = new SampleTarget.Data();
            data.sample = new Sample.Data();
            data.sample.id = this.data_._id;
            data.sample.url = this.data_._objUrl;
            data.sample.version = this.data_._version;
            data.sample.size = this.data_._size;
          
            return data;
        }


    }
}