using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    public class ResourceManagber : GDGeek.Singleton<ResourceManagber>
    {   
        internal void holoTransform(int id, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            Resource[] resourcies = Component.FindObjectsOfType<Resource>();
            for (int i = 0; i < resourcies.Length; ++i) {
                if (resourcies[i]._id == id)
                {
                    resourcies[i].transform.localPosition = localPosition;
                    resourcies[i].transform.localRotation = localRotation;
                    resourcies[i].transform.localScale = localScale;
                }
            }
        }
    }
}