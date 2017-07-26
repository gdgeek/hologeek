using GDGeek;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public abstract class WindowBase : MonoBehaviour
    {
        public enum Mode
        {

            Item,
            Save,
            Load,
            None,
        };
        public string _title;
        public abstract int pages { get;}
        public abstract int page { get;  set; }
        public abstract bool isBusy{ get; }

        public abstract Task refresh();
        public abstract Task loading();
    }
}
