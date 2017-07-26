using GDGeek;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTest : MonoBehaviour {
    public GameObject _obj;
	// Use this for initialization
	void Start () {
        TaskCircle tl = new TaskCircle();
        GameObject obj = null;
        Task tw = new TaskWait(3.0f);
        TaskManager.PushBack(tw, delegate
        {
           obj  = GameObject.Instantiate(_obj);
            obj.transform.position = new Vector3(0, 0, 2);
        });

        tl.push(tw);
        Task tw2 = new TaskWait(3.0f);
        TaskManager.PushBack(tw2, delegate
        {
            if (obj != null) {
                Destroy(obj);
            }
        });

        tl.push(tw2);

#if UNITY_EDITOR
        TaskManager.Run(tl);
#endif
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
