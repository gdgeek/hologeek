using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test3rd : MonoBehaviour {

    void Awake()
    {

        var scene = SceneManager.GetSceneByName("HoloMenu3rd");
        if(scene != null) { 
            SceneManager.LoadScene("HoloMenu3rd", LoadSceneMode.Additive);
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("HoloMenu3rd"));
        //SceneManager.
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
