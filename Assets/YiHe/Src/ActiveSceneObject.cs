using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveSceneObject : MonoBehaviour {
    void Awake() {
        SceneManager.activeSceneChanged += delegate (Scene old, Scene curr)
        {
            Debug.Log("old" + old.name);
            Debug.Log("curr" + curr.name);
          

            if (curr != null) {
                GameObject temp = new GameObject("Temp");
                this.transform.SetParent(temp.transform);
                this.transform.SetParent(null);
                Destroy(temp);
            }
        };
    }
	
}
