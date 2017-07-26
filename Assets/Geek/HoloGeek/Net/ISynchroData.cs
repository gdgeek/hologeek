using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek {
    /// <summary>
    /// 同步数据用来检查数据是否脏了，以及清扫接口
    /// </summary>
    public interface ISynchroData {


        bool dirty();
        void sweep();
    }
}