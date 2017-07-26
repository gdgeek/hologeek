using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    namespace Net { 
        public interface IServer{
            void functor(Functor.Handler handler);
            void synchro(IMessageWriter writer);
            void createObj(IMessageWriter writer);
            void destoryObj(string shareId);

        }
    }
}