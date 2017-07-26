using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek
{
    public class UIManager : GDGeek.Singleton<UIManager>//MonoBehaviour
    {
        public GameObject _ui;
        public GameObject _stick;
        private Choose choose_ = null;
        public HoloMoving _moving;
        private HoloMoving oldMoving_;
        private void index2Action(int idx) {
            switch (idx)
            {
                case 0:
                    onDrag();
                    break;
                case 1:
                    onRotation();
                    break;
                case 2:
                    onZoom();
                    break;
            }
        }
        void Start()
        {
            choose_ = this.gameObject.GetComponentInChildren<Choose>();
            UnityEngine.Events.UnityAction<int> adction = new UnityEngine.Events.UnityAction<int>(delegate(int i) {
                index2Action(i);
            });

            choose_._onChose.AddListener(adction);
            if (_moving != null) {
                index2Action(choose_._index);
            }
            close();
        }
        public void onDrag()
        {
            if (_moving != null)
            {
                _moving._mode = HoloMoving.Mode.Drag;
            }
        }
        public void onRotation()
        {
            if (_moving != null)
            {
                _moving._mode = HoloMoving.Mode.Rotation;
            }
        }
        public void onZoom()
        {

            if (_moving != null)
            {
                _moving._mode = HoloMoving.Mode.Zoom;
            }
        }


        public void onNone()
        {

            if (_moving != null)
            {
                _moving._mode = HoloMoving.Mode.None;
            }
        }
        public void onReset() {

            if (_moving != null)
            {
                _moving.reset();
            }
        }
        public void link(HoloMoving moving) {

            if (_moving != null)
            {
                onNone();
                _moving = null;
            }

            _moving = moving;
            choose_.reset();
          
        }
        private void close()
        {
            _ui.SetActive(false);
        }
        private void open()
        {
            _ui.SetActive(true);
        }
        void Update()
        {
            if (oldMoving_ != _moving) {

                if (_moving == null)
                {
                    close();
                }
                else {
                    open();
                }
                oldMoving_ = _moving;
            }
            if (_moving != null)
            { 
                var target = _moving._object.GetComponent<Collider>();
                Vector3 point = target.ClosestPoint(_stick.transform.position);
                var length = (_stick.transform.position - this.transform.position).normalized;
                _ui.transform.position = point + length * 0.05f;
                this.gameObject.transform.position = target.transform.position;
            }
        }
    }
}