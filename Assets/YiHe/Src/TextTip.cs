using GDGeek;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YiHe
{
    public class TextTip : MonoBehaviour
    {
        public TextMesh _text = null;
        public Transform _textPoint = null;

        Sample.Data data_;
        public Vector3 _modelSize;
        public Transform _target;

        private void Start()
        {
            _text.transform.position = this._textPoint.position;
        }

        public Task loadingComplete(Sample.Data data)
        {
            data_ = data;

            _modelSize = data_.size;

            return new Task()
            {
                init = () =>
                {
                    textOffset = new Vector3(0f, -0.1f * data_.radius, 0f);
                    this.transform.Translate(textOffset);
                },
                update = sizeUpdate
            };
        }

        private void sizeUpdate(float d)
        {
            _target = _text.transform.parent.transform;
            _text.text = string.Format("尺寸:{0} x {1} x {2} (1:{3:0.00})",
                                        (int)(_modelSize.x * _target.localScale.x * 100),
                                        (int)(_modelSize.y * _target.localScale.y * 100),
                                        (int)(_modelSize.z * _target.localScale.z * 100),
                                        _target.localScale.x);
        }


        protected void Update()
        {
            if (_text != null)
            {
                sizeUpdate(1f);
            }
        }

        private Vector3 textOffset = Vector3.zero;
    }

}