using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;
using UnityEngine.Events;

namespace HoloGeek {
    /*
      public interface IFocusable : IEventSystemHandler
    {
        void OnFocusEnter();
        void OnFocusExit();
         
         */
    public class Button : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler, HoloToolkit.Unity.InputModule.IFocusable, HoloToolkit.Unity.InputModule.IInputHandler
    {

        public Material _selectMaterial;
        public Material _normalMaterial;
        public Renderer _renderer;
        public UnityEvent _onClicked = new UnityEvent();

        public GameObject _offset;

        public void OnInputUp(HoloToolkit.Unity.InputModule.InputEventData eventData) {

            TweenLocalPosition.Begin(_offset.gameObject, 0f, new Vector3(0, 0, 0));

        }
        public void OnInputDown(HoloToolkit.Unity.InputModule.InputEventData eventData)
        {
            TweenLocalPosition.Begin(_offset.gameObject, 0.1f, new Vector3(0, 0, 0.1f));
        }
        public void OnInputClicked(HoloToolkit.Unity.InputModule.InputClickedEventData eventData) {
            _onClicked.Invoke();
        }

        public void OnFocusEnter() {
            TweenScale.Begin(_offset, 0.02f, Vector3.one * 1.2f);

        }
        public void OnFocusExit() {

            TweenScale.Begin(_offset, 0.0f, Vector3.one);
            TweenLocalPosition.Begin(_offset.gameObject, 0f, new Vector3(0, 0, 0));
        }
        public virtual void on() {
            _renderer.material = _selectMaterial;
        }
        public virtual void off() {

            _renderer.material = _normalMaterial;
        }
    }
}