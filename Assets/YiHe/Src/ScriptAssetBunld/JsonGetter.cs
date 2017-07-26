using UnityEngine;
using System.Collections;
using GDGeek;
using System;

public class JsonGetter : Singleton<JsonGetter>
{
    public IEnumerator getJson(string jsonUrl, Action<string> onDownloadJsonComplete)
    {
        Debug.Log("url is " + jsonUrl);
        string json = null;

        using (WWW asset = new WWW(jsonUrl))
        {
            yield return asset;
            if (asset != null)
            {
                AssetBundle bundle = asset.assetBundle;

                json = asset.text;

                if (onDownloadJsonComplete != null && json != "")
                {
                    onDownloadJsonComplete(json);
                }
                else
                {
                    Debug.Log("json is null");
                }
            }
        }
    }
}
