using UnityEngine;
using System.Collections;
using GDGeek;

namespace YiHe
{

    public class GlobalManager : Singleton<GlobalManager>
    {
        public float unit = 0.01f;
        public readonly string proofModelSceneListJsonUrl = "http://101.37.149.220/HololensWebYiHe/web/proof-model-scene/list";
        internal readonly string sceneUploadUrl = "http://101.37.149.220/HololensWebYiHe/web/proof-model-scene/upload";
        internal readonly string sceneGetUrl = "http://101.37.149.220/HololensWebYiHe/web/proof-model-scene/json";
    }
}

