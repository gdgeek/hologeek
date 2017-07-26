using System;
using UnityEngine;

public class LoadData 
{
    public LoadData(DateTime dateTime, int id)
    {
        this._date = dateTime;
        this.id = id;
    }

    [SerializeField]
    public DateTime _date;
    [SerializeField]
    private int _n;

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

    public LoadData(int n)
    {
        id = n;
    }

}