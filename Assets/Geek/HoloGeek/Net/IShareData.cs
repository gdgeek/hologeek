using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public interface IShareData  {

        IMessageWriter getWriter();
        IMessageReader getReader();
    }
}