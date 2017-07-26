using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YiHe { 
    public class ButtonGroupManager : MonoBehaviour
    {

        public List<GameObject> ButtonGroup;

        private GameObject lastPressedButton;

        private void Start()
        {
            var button = ButtonGroup.Find((obj) => obj.name == "Model");
            switchToThisItem(button);
        }

        public void switchToThisItem(GameObject item)
        {
            if (!ButtonGroup.Contains(item))
            {
                return;
            }
            if (lastPressedButton != null)
            {
                lastPressedButton.GetComponent<StateItem>().changeState();
            }
            item.GetComponent<StateItem>().changeState();
            lastPressedButton = item;
        }

    }
}