using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloGeek.Snapshot;
namespace YiHe {
    public class SampleTarget : Target
    {
        public string _type;
        public override string type
        {
            get
            {
                return _type;
            }
        }

        [Serializable]
        public class Data : Target.IParameter
        {
            [SerializeField]
            public Sample.Data sample;


            public string toJson()
            {
                return JsonUtility.ToJson(this);
            }
        }
      
        public override Target create(Target.IParameter parameter)
        {
            Data data = parameter as Data;
            if (data != null)
            {
                return createImpl(data);
            }
            return null;
        }

        private Target createImpl(Data data)
        {
            this.gameObject.SetActive(false);
            Target target = GameObject.Instantiate(this);
            target.transform.SetParent(HoloGeek.Snapshot.Root.Instance.transform);
            Sample manager = target.gameObject.GetComponent<Sample>();
            manager.data = data.sample;
            target.gameObject.SetActive(true);
            return this;
        }

        public override Target create(string json)
        {
            return createImpl(JsonUtility.FromJson<Data>(json));
        }


        public override string toJson(Target target)
        {
            Sample manager = target.gameObject.GetComponent<Sample>();
        
            Data data = new Data();
            data.sample = manager.data;
            return data.toJson();
        }

       
    }

}