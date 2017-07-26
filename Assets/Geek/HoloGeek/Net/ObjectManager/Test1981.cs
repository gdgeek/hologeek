using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1981 : MonoBehaviour {

    public void OnDestroy()
    {
        Debug.Log("1981 destory");
    }
    public void OnDisable()
    {
        Debug.Log("1981 Disable");
    }
    public void OnApplicationQuit() {
        Debug.Log("=======");
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
