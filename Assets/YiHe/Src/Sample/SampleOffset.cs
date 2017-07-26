using GDGeek;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public class SampleOffset : MonoBehaviour {
        private float radius_;
        private float max_ = 1f;
        private float redio_ = 1.0f;
        internal float radius
        {
            get
            {
                return radius_;
            }

           
        }
        private void refresh() {

            float scale = radius_ * 2 / max_;
            this.transform.localScale = Vector3.one * scale * redio_;
        }

        internal void setObj(GameObject obj)
        {

            BoxCollider bc = obj.gameObject.GetComponent<BoxCollider>();
            if (bc == null) {
                bc = obj.gameObject.AddComponent<BoxCollider>();
            }
            max_ = Mathf.Max(Mathf.Max(bc.size.x, bc.size.y), bc.size.z);
            obj.transform.SetParent(this.transform);
            obj.transform.localScale = Vector3.one;
            refresh();
            obj.transform.localPosition = -bc.center;
            obj.gameObject.SetActive(true);
            DestroyImmediate(bc);

        }

        internal void setRadius(float radius)
        {
            radius_ = radius;
            refresh();
        }

        internal void scale(float redio)
        {
          
            this.redio_ = redio;
            refresh();
        }

        public Task show()
        {
            TweenTask tt = new TweenTask(delegate
            {
                
             
                return TweenValue.Begin(this.gameObject, 0.3f, 0, 1, delegate (float r)
                {
                    
                    this.scale(r);

                });
            });

          


            return tt;
        }
    }
}