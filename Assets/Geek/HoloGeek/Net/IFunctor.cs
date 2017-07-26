using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public interface IFunctor
    {
        
        string shareId
        {
            get;
        }

        void callback(string func, string parameter);
    }

}
