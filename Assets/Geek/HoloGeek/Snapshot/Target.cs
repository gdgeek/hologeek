using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek
{
    namespace Snapshot
    {
        public abstract class Target : MonoBehaviour
        {
           /* public string _type;

            public string type
            {
                get
                {
                    return _type;
                }

                set
                {
                    _type = value;
                }
            }*/
            public interface IParameter
            {
                string toJson();
            }


           // public Target _phototype;
            public abstract string type { get; }
            public abstract Target create(string json);
            public abstract Target create(IParameter parameter);
            public abstract string toJson(Target obj);


        }



    }
}
