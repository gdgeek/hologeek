using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    namespace Net { 
        public interface ISynchro : ISynchroData {
            string shareId
            {
                get;
            }
            IMessageWriter getDirtyWriter();
            IMessageReader getDirtyReader();
        }
    }
}