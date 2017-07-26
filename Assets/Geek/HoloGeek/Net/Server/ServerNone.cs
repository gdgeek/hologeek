using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek
{
    namespace Net { 
        public class ServerNone : MonoBehaviour, IServer
        {
            public void functor(Functor.Handler handler)
            {
           
            }

            public void synchro(IMessageWriter writer)
            {
          
            }

            public void createObj(IMessageWriter writer)
            {
           
            }

            public void destoryObj(string shareId)
            {
          
            }
        }
    }
}
