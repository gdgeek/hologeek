using HoloGeek.Snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTest : MonoBehaviour {

	// Use this for initialization
	void Start () {


        YiHe.SampleTarget.Data data = new YiHe.SampleTarget.Data();
        data.sample = new YiHe.Sample.Data();
        data.sample.id = 123;
        //data.radius = 0.5f;

        data.sample.url = "http://101.37.149.220/HololensWebYiHe/web/uploads/TableBig.assetbundle";
        data = JsonUtility.FromJson<YiHe.SampleTarget.Data>(JsonUtility.ToJson(data));
        Debug.Log(JsonUtility.ToJson(data));
        PhotoTransform pt = new PhotoTransform();
        pt.localScale = Vector3.one;
        Target target = HoloGeek.Snapshot.Lens.Create("YiHeObj", data, pt);
        var json = Lens.TakePhoto();
        Debug.Log(json);
        Root.Instance.deleteAllTarget();
        Lens.DevelopPhoto(json);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
