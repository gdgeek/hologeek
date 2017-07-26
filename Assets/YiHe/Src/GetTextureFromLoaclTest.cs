using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTextureFromLoaclTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadTex());	
	}

    private IEnumerator LoadTex()
    {
        using (var www = new WWW("file://E:/HoloProject/HoloGeek/Assets/StreamingAssets/Desk.png"))
        {
            yield return www;
            if (www != null)
            {
                Debug.Log("not null");
                if (www.isDone && www.error == null)
                {
                    Debug.Log("texuter OK!" + www.texture);
                }
                else
                {
                    Debug.Log(www.error);
                }
            }
            else
            {
                Debug.Log("null!");
            }
        }
    }
}
