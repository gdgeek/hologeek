using System;
using System.Collections;
using System.Collections.Generic;
using GDGeek;
using UnityEngine;
namespace YiHe
{
    public class SampleLoader : GDGeek.Singleton<SampleLoader>
    {

        public class LoadTask : Task
        {
            public GameObject _obj = null;

        };
        public static void CreateBox(GameObject gameObj)
        {
            Transform parent = gameObj.transform;
            Vector3 postion = parent.position;
            Quaternion rotation = parent.rotation;
            Vector3 scale = parent.localScale;
            parent.position = Vector3.zero;
            parent.rotation = Quaternion.Euler(Vector3.zero);
            parent.localScale = Vector3.one;

            Collider[] colliders = parent.GetComponentsInChildren<Collider>();
            foreach (Collider child in colliders)
            {
                DestroyImmediate(child);
            }
            Vector3 center = Vector3.zero;
            Renderer[] renders = parent.GetComponentsInChildren<Renderer>();
            foreach (Renderer child in renders)
            {
                center += child.bounds.center;
            }
            center /= parent.GetComponentsInChildren<Renderer>().Length;
            Bounds bounds = new Bounds(center, Vector3.zero);


            BoxCollider boxCollider = parent.gameObject.AddComponent<BoxCollider>();
            Renderer renderer = gameObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                bounds.Encapsulate(boxCollider.bounds);
            }

            foreach (Renderer child in renders)
            {
                bounds.Encapsulate(child.bounds);
            }
            boxCollider.center = bounds.center - parent.position;
            boxCollider.size = bounds.size;

            parent.position = postion;
            parent.rotation = rotation;
            parent.localScale = scale;
        }

        internal LoadTask load(string url, int version)
        {
            Debug.Log(url);
            bool isOver = false;
            LoadTask task = new LoadTask();
            task.init = delegate
            {
                ModelLoader.Instance.StartLoadModelFromUrl(version, url, this.transform, delegate (GameObject gameObj)
                {
                    gameObj.SetActive(true);
                    isOver = true;

                    CreateBox(gameObj);
                   
                    task._obj = gameObj;
                    gameObj.SetActive(false);
                });
              
            };

            task.isOver = delegate
            {
                return isOver;
            };
            return task;

        }




    }
}