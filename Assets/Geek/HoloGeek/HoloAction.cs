using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoloGeek { 
    public interface IAction {
        void execute(HoloToolkit.Sharing.User sender);
    }
}