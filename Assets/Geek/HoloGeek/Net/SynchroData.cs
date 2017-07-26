using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public abstract class SynchroData : ShareData, ISynchroData  {
        public abstract bool dirty();

        public abstract void sweep();
        
    }
}
