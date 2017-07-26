using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneObjInfoManagerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

        CloneObjInfoManager.Instance.addObjInList(this.gameObject);

    }

    private void OnDestroy()
    {

        if (CloneObjInfoManager.HasInstance) { 
            CloneObjInfoManager.Instance.removeObjFromList(this.gameObject);
        }
    }
}
