using GDGeek;
using HUX.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YiHe
{
    /// <summary>
    /// 打样窗口控制类
    /// </summary>
    /// <seealso cref="YiHe.WindowBase" />
    public class SampleWindow : WindowBase
    {
        public SampleItem[] _items = null;
        public SampleData[] _datas;
        private bool isBusy_ = false;
       
        public override int pages
        {
            get
            {

                if (this._datas == null || this._datas.Length == 0)
                {
                    return 1;
                }
                return (this._datas.Length-1)/this._items.Length +1;
            }

          
        }
        private int page_ = 0;
        public override int page {

            get {
                return page_;
            }

            set {
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

       
        private Task loadingTask(int i, SampleData data)
        {
            return _items[i].loading(data);

        }
        private Task refresh_() {


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

        private Task loading_() {

            Task task = new Task();
            bool isOver = false;
            TaskManager.PushFront(task, delegate
            {
                _items = this.GetComponentsInChildren<SampleItem>();

                isBusy_ = true;
                JsonLoader.Instance.startGetJson(delegate (string json)
                {


                    JsonModelData data = JsonUtility.FromJson<JsonModelData>(json);
                    var modelsinfo = data.jsonInfo.modelsInfo;
                    var jsondate = data.jsonInfo.modelsInfo;
                    _datas = new SampleData[modelsinfo.Length];
                    for (int i = 0; i < modelsinfo.Length; ++i)
                    {
                        SampleData d = new SampleData();
                        d._id = modelsinfo[i].id;
                        d._imageUrl = modelsinfo[i].imageUrl;
                        d._objUrl = modelsinfo[i].url;
                        d._version = modelsinfo[i].version;
                       // d._radius = modelsinfo[i].colliderRadius;
                        d._size = new Vector3(modelsinfo[i].x, modelsinfo[i].y, modelsinfo[i].z);
                        _datas[i] = d;
                    }
                    isOver = true;

                });
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
        public override Task loading()
        {
            return new TaskPack(delegate {
                return loading_();
            });
        }
    }

}

