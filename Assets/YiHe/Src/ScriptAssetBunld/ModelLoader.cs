using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using GDGeek;
public class ModelLoader : Singleton<ModelLoader>
{
    //private bool isDownLoading_ = false;
    /*
        public static readonly string PathURL =
    #if UNITY_ANDROID
            "jar:file://" + Application.dataPath + "!/assets/";
    #elif UNITY_IPHONE
            Application.dataPath + "/Raw/";
    #elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        "file://" + Application.dataPath + "/StreamingAssets/";
    #else
            string.Empty;
    #endif
    */
    public int maxLoadTime = 10;
    private string lastBundleUrl = "";
    public void StartLoadModelFromUrl(int version, string BundleURL, Transform parent, Action<GameObject> OnDownloadOver)
    {
        //if (isDownLoading_)
        //{
        //    return;
        //}
        //isDownLoading_ = true;
        HoloDebug.Log("Start Download");
        StartCoroutine(DownloadAssetAndScene(version, BundleURL, parent, OnDownloadOver));
    }


    IEnumerator DownloadAssetAndScene(int version, string BundleURL, Transform parent, Action<GameObject> OnDownloadOver)
    {
        //下载assetbundle，加载Cube  
        int loadtime = maxLoadTime;
        while (true)
        {
            if (loadtime == 0)
            {
                Debug.Log("Load Time Over!");
                break;
            }
            HoloDebug.Log("url is " + BundleURL + "version:" + version);
            WWW downLoad = WWW.LoadFromCacheOrDownload(BundleURL, version);
            yield return downLoad;
            if (downLoad.isDone)  //WWW asset = new WWW(BundleURL)
            {
                //yield return asset;
                AssetBundle bundle = downLoad.assetBundle;

                var gameobj = (GameObject)Instantiate(bundle.mainAsset, parent);
                //gameobj.transform.localPosition = new Vector3(0, 0, 10);
                //var meshClone = gameobj.GetComponentInChildren<MeshRenderer>();
                //meshClone.gameObject.AddComponent<BoxCollider>();
                //meshClone.gameObject.AddComponent<TapToPlace>();
                //var tapToPlace = meshClone.GetComponent<TapToPlace>();
                //tapToPlace.ParentGameObjectToPlace = gameobj;
                //tapToPlace.PlaceParentOnTap = true;
                //tapToPlace.IsBeingPlaced = true;


                //yield return null;  
                //Debug.Log(gameobj);
                bundle.Unload(false);
                //isDownLoading_ = false;
                if (OnDownloadOver != null)
                {
                    OnDownloadOver.Invoke(gameobj);
                    break;
                }
            }
            else
            {
                --loadtime;
                yield return new WaitForEndOfFrame();
                continue;
            }
        }
    }
}