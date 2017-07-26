using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SaveData{
    [SerializeField]
    public DateTime _date;
    [SerializeField]
    private int _n;
    [SerializeField]
    public bool _isset;

    public int id
    {
        get
        {
            return _n;
        }

        set
        {
            _n = value;
        }
    }

    public SaveData(int n)
    {
         id = n;
        _isset = false;
    }

    public SaveData(DateTime date, int n)
    {

        _isset = true;
        _date = date;
        this.id = n;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
