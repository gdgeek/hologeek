using ModelHolo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace YiHe
{
    [ExecuteInEditMode]
    public class JsonLoader : GDGeek.Singleton<JsonLoader> {
	    public string BundleURL = "";
        private string json_ = "";
        private readonly string jsonKey = "localJson";
        public bool _isClean = false;

        // Use this for initialization
        private Action<string> OnDownloadJsonComplete;

        IEnumerator DownloadAssetAndScene()
        {
            //下载assetbundle，加载Cube  
            Debug.Log("url is " + BundleURL);
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                loadJsonFromLocal();
                Debug.Log("json data " + json_);
                if (OnDownloadJsonComplete != null && json_ != "")
                {
                    OnDownloadJsonComplete(json_);
                }
                else
                {
                    Debug.Log(" nulll...... OnDownloadJsonComplete");
                }
            }
            else
            {
                while (true)
                {
                    using (WWW asset = new WWW(BundleURL))
                    {
                        yield return asset;
                        if (asset != null)
                        {
                            AssetBundle bundle = asset.assetBundle;

                            json_ = asset.text;
                        

                            if (OnDownloadJsonComplete != null && json_ != "")
                            {
                                saveJsonToLocal(json_);
                                OnDownloadJsonComplete(json_);
                                break;

                            }
                            else
                            {
                                Debug.Log(" nulll...... OnDownloadJsonComplete");
                            }
                        }
                        else
                        {

                            continue;
                        }

                    }
                }
            }
        
        
        }

        private void loadJsonFromLocal()
        {
           json_ = PlayerPrefs.GetString(jsonKey);
        }

        private void saveJsonToLocal(string json)
        {
            var data = JsonUtility.FromJson<JsonModelData>(json);
            var models = data.jsonInfo.modelsInfo.ToList();
  
            //foreach (var model in models)
            //{
            //    model.imageUrl = "file://" + Application.dataPath + "/StreamingAssets/" + model.name.Split('.')[0] + ".png";
            //}
            json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(jsonKey, json);
        }

        internal void startGetJson(Action<string> OnDownloadJsonComplete)
        {
            this.OnDownloadJsonComplete = OnDownloadJsonComplete;
            StartCoroutine(DownloadAssetAndScene());
        }

    
        void Update()
        {
            if(_isClean)
            {
                Debug.Log("clean is ok");
                PlayerPrefs.DeleteAll();
                _isClean = false;
            }
        
        }
    }
}