using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ObjCalculate : MonoBehaviour {
    public bool _refresh = false;
    public float _radius = 0.05f;
  
	
	// Update is called once per frame
	void Update () {
        if (_refresh) {
            _refresh = false;

            BoxCollider bc = this.gameObject.AddComponent<BoxCollider>();
            
            float max = Mathf.Max(Mathf.Max(bc.size.x, bc.size.y), bc.size.z);
            float r = _radius*2 / max;
            this.transform.localScale = Vector3.one * r;
            DestroyImmediate(bc);
          
            SphereCollider sc = this.gameObject.AddComponent<SphereCollider>();
          
            this.transform.localPosition = -sc.center * r;
            DestroyImmediate(sc);
          
        }
	}
}
