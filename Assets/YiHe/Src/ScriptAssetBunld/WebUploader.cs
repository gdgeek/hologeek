using UnityEngine;
using System.Collections;
using GDGeek;
using System;
using System.Net;

public class WebUploader : Singleton<WebUploader>
{
    public IEnumerator uploadScene(string url, int id, int datestamp, string json, Action OnUploadComplete)
    {
        var form = new WWWForm();
        form.AddField("id", id);
        form.AddField("upload_date", datestamp);
        form.AddField("json", json);
        //form.headers["methons"] = "post";
        var www = new WWW(url, form);

        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Upload Error : " + www.error + www.text);
        }
        else
        {
            if (OnUploadComplete != null)
            {
                OnUploadComplete();
            }
        }

    }
}
