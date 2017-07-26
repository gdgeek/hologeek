using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ModelHolo
{
    [Serializable]
    public class ModelInfo
    {
        public int id;
        public string name;
        public string url;
        public string imageUrl;
        public string updateDate;
        public Texture texture;
        public int version;
        public float colliderRadius;
        public float x, y, z;
    }
}
