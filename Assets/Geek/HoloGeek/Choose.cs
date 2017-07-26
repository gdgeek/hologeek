using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HoloGeek {

    public class ChooseEvent : UnityEvent<int> { }

    public class Choose : MonoBehaviour {
        public int _index = 0;
        public List<Button> _buttons;
        public ChooseEvent _onChose = new ChooseEvent();
        public void Start() {
            for (int i = 0; i < _buttons.Count; ++i) {
                _buttons[i]._onClicked.AddListener(this.onButtonClicked(i));
            }
            refresh();
        }
        UnityAction onButtonClicked(int idx) {
            var action = new UnityAction(delegate {
                clicked(idx);
              
            });

            return action;
        }

        private void clicked(int idx)
        {
            _index = idx;
            refresh();
            _onChose.Invoke(idx);
        }

        void refresh() {
            for (int i = 0; i < _buttons.Count; ++i) {
                if (i == _index)
                {
                    _buttons[i].on();
                }
                else {
                    _buttons[i].off();
                }
            }
        }

        internal void reset()
        {
            clicked(0);
        }
    }
}