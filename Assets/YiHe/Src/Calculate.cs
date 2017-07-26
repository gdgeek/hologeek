using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Calculate : MonoBehaviour {

    public float h = 0.08f;
    public float w = 0.08f;
    public float it = 0.0004f;
    public float xn = 5;
    public float yn = 0.5f;
    public float x = 0;
    public float y = 3;
    public float s = 2;


	
	// Update is called once per frame
	void Update () {
        BoxCollider box = this.gameObject.GetComponent<BoxCollider>();
        Debug.Log(box.size);
        float width = (w + it) * xn * s;
        float height = (h + it) * yn * s;
        this.transform.localPosition = new Vector3(box.size.x /2 * (width / box.size.x) + (w + it) * x * s, (box.size.y / 2 *(height/box.size.y) + (h + it )*y * s) , 0);
      
        this.transform.localScale = new Vector3(width/box.size.x, height / box.size.y, 1);
    }
}
