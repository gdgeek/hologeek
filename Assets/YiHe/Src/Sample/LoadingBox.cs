using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
using System;

namespace YiHe
{
    public class LoadingBox : MonoBehaviour
    {
        private Vector3 revise_ = Vector3.one;
        private float radius_ = 1.0f;
        // Use this for initialization
        void Start()
        {
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            revise_ = new Vector3(1f / box.size.x, 1f / box.size.y, 1f / box.size.z);
            DestroyImmediate(box);
            refresh();
        }

        private void refresh()
        {
            this.transform.localScale = revise_ * radius_ * 2;
        }

        public void setRadius(float radius)
        {
            radius_ = radius;
            refresh();
        }
        // Update is called once per frame
        void Update()
        {

            var r = Tween.easeIt(Tween.Method.easeInOutBack, 0, 1, Mathf.Clamp01(Time.deltaTime * 360));
            this.transform.Rotate(new Vector3(0, -r, 0));
        }

        public Task hide()
        {
            TweenTask tt = new TweenTask(delegate
            {
                Renderer renderer = this.GetComponent<Renderer>();
                Color color = renderer.material.color;
                return TweenValue.Begin(this.gameObject, 0.3f, 0, 1, delegate (float r)
                {
                    renderer.material.color = new Color(color.r, color.g, color.b, 1f - r);

                });
            });

            TaskManager.PushBack(tt, delegate
            {
                if(this!= null && gameObject != null) { 
                    gameObject.SetActive(false);
                }
            });
            return tt;
        }
    }
}