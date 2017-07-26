using GDGeek;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YiHe;

public class LoadWindow : WindowBase {
    public LoadItem[] _items = null;

    public LoadData[] _datas;
    private bool isBusy_ = false;

    public override int pages
    {
        get
        {

            if (this._datas == null || this._datas.Length == 0)
            {
                return 1;
            }
            return (this._datas.Length - 1) / this._items.Length + 1;         
        }


    }
    private int page_ = 0;
    public override int page
    {

        get
        {
            return page_;
        }

        set
        {
            page_ = value;
        }
    }

    public override bool isBusy
    {
        get
        {
            return isBusy_;
        }
    }

    private Task closeTask(int i)
    {
        return _items[i].closeing();
    }

    private Task loadingTask(int i, LoadData data)
    {
        return _items[i].loading(data);
    }
    private Task refresh_()
    {
        if (isBusy)
        {
            return new Task();
        }

        TaskSet ts = new TaskSet();
        TaskManager.PushFront(ts, delegate
        {
            isBusy_ = true;
        });

        TaskManager.PushBack(ts, delegate
        {
            isBusy_ = false;
        });
        Debug.Log("_items.Length:" + _items.Length);
        for (int i = 0; i < _items.Length; ++i)
        {
            int n = page * _items.Length;
            int s = n + i;
            Debug.Log("s:" + s);
            Debug.Log("page:" + page);
            if (s < _datas.Length)
            {
                ts.push(loadingTask(i, _datas[s]));
            }
            else
            {
                ts.push(closeTask(i));
            }

        }
        return ts;
    }

    public override Task refresh()
    {
        return new TaskPack(delegate
        {
            return refresh_();
        });
    }
    [Serializable]
    class JsonSingleData
    {
        public int id;
        public UInt64 upload_date;
    }
    [Serializable]
    class JsonData
    {
        public JsonSingleData[] datas;
    }

    public override Task loading()
    {
        Task task = new Task();
        bool isOver = false;
        TaskManager.PushFront(task, delegate
        {
            _items = this.GetComponentsInChildren<LoadItem>();
            isBusy_ = true;
            StartCoroutine(JsonGetter.Instance.getJson(GlobalManager.Instance.proofModelSceneListJsonUrl, delegate (string json)
            {
                var jsonData = JsonUtility.FromJson<JsonData>(json);
                var loadDatas = new List<LoadData>();
                foreach (var data in jsonData.datas)
                {
                    loadDatas.Add(new LoadData(TimeUtility.FromTimeStamp(data.upload_date.ToString()), data.id));
                }
                loadDatas.Reverse();
                Debug.Log(jsonData);
                this._datas = loadDatas.ToArray();
                isOver = true;
                
            }));
        });
        TaskManager.AddAndIsOver(task, delegate
        {
            return isOver;
        });

        TaskManager.PushBack(task, delegate
        {
            isBusy_ = false;
        });

        return task;

    }
}

