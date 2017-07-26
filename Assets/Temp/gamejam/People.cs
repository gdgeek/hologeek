using HoloToolkit.Sharing;
using SpectatorView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class People : MonoBehaviour {
    public string _id= "";
	// Use this for initialization
	void Start () {
		
	}

    float r = 0;
	// Update is called once per frame
	void Update () {
        r = Time.deltaTime*100;
        this.transform.Rotate(0, 0, r);
	}
   
     // Called whenever this object is involved in a collision.
    void OnCollisionEnter(Collision collision)
    {
        
        if (PeopleManager.Instance.isSelf())
        { 
            // We only care if the collision is with a projectile.
            ProjectileBehavior pb = collision.contacts[0].otherCollider.gameObject.GetComponent<ProjectileBehavior>();
            if (pb != null)
            {
                PeopleManager.Instance.onHit(this._id);
            }
           
        }

    }
}
