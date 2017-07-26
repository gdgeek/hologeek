using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public class HideItem : StateItem {
        public GameObject HideIcon, ShowIcon;
        public TextMesh textMesh;

        protected override void press()
        {
            //base.press();
            HideIcon.SetActive(true);
            ShowIcon.SetActive(false);
            textMesh.text = "Hide";
        }

        protected override void unpress()
        {
           // base.unPress();
            ShowIcon.SetActive(true);
            HideIcon.SetActive(false);
            textMesh.text = "Show";

        }
    }
}