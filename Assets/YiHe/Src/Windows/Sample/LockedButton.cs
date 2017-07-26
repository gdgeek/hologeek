using UnityEngine;
using System.Collections;
namespace YiHe { 
    public class LockedButton : MonoBehaviour
    {
        public GameObject PressedIcon, UnPressedIcon;

        public void press()
        {
            UnPressedIcon.SetActive(true);
            PressedIcon.SetActive(false);
        }
        public void unpress()
        {
            PressedIcon.SetActive(true);
            UnPressedIcon.SetActive(false);
        }
        public void setEnabled(bool enabled) {
            if (enabled)
            {
                press();
            }
            else {
                unpress();
            }
        }
    }

}