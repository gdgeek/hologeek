using GDGeek;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum InfoShow
{
    Hide,
    Show
}

public class CloneObjInfoManager : Singleton<CloneObjInfoManager> {


    public InfoShow isShow = InfoShow.Show;
    private List<GameObject> CloneObjs;
    public GameObject frameObj, appBar;

    private void Awake()
    {
        CloneObjs = new List<GameObject>();
    }


    
    private void Update()
    {
        if (frameObj == null)
        {
            frameObj = GameObject.Find("BoundingBoxShell(Clone)");
        }
        if (appBar == null)
        {
            appBar = GameObject.Find("AppBar(Clone)");
        }
    }


    
    public void addObjInList(GameObject obj)
    {
        if (this.isShow == InfoShow.Hide) {
            this.Hide(gameObject);
        }
        CloneObjs.Add(obj);
    }

    public void removeObjFromList(GameObject obj) {
        CloneObjs.Remove(obj);
    }

    public void Hide(GameObject obj)
    {
        obj.GetComponentInChildren<TextMesh>().gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void show(GameObject obj)
    {
        obj.GetComponentInChildren<TextMesh>().gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    
    public void setHide(bool hide)
    {
        if (hide)
        {
            isShow = InfoShow.Hide;
            isShow = InfoShow.Hide;
            frameObj.SetActive(false);
            appBar.SetActive(false);
            foreach (GameObject temp in CloneObjs)
            {
                Hide(temp);
            }
        }
        else {
            isShow = InfoShow.Show;
            frameObj.SetActive(true);
            appBar.SetActive(true);
            foreach (GameObject temp in CloneObjs)
            {
                show(temp);
            }
        }
      
    }
    public void clickHideButton()
    {
        if (isShow == InfoShow.Show)
        {
            isShow = InfoShow.Hide;
            frameObj.SetActive(false);
            appBar.SetActive(false);
            foreach (GameObject temp in CloneObjs)
            {
                Hide(temp);
            }
        }
        else
        {
            isShow = InfoShow.Show;
            frameObj.SetActive(true);
            appBar.SetActive(true);
            foreach (GameObject temp in CloneObjs)
            {
                show(temp);
            }
        }
    }

}
