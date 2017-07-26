using HoloGeek;
using HoloGeek.Net;
using HUX.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YiHe
{
    public class LockedCtrl: HUX.Receivers.InteractionReceiver
    {

        public OwnerPointer _owner;//
        public LockedButton _button;
        public bool _isEnabled = false;

        public HoloToolkit.Unity.SimpleTagalong _tagalong;
        public void Start()
        {
            setEnabled(_isEnabled);
            _owner.onRefresh += delegate//
            {
                if (_owner.isOwner())
                {

                    setEnabled(true);
                }
                else {

                    setEnabled(false);
                }
            };
        }
        public void setEnabled(bool enabled) {
            _isEnabled = enabled;
            _button.setEnabled(_isEnabled);
            _tagalong.enabled = _isEnabled;
        }
        protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
        {
            LockedButton button = obj.GetComponent<LockedButton>();
            if (button != null) {
                setEnabled(!_isEnabled);
                if (_isEnabled) {
                    _owner.occupy();
                }
            }
        }

     
    }
}