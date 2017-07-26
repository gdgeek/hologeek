using HoloToolkit.Examples.InteractiveElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace HoloGeek { 
    public class HoloMoving : GestureInteractiveControl
    {

        public GameObject _object;
        public enum Mode{
            None,
            Select,
            Drag,
            Rotation,
            Zoom,
        }
        public Mode _mode = Mode.None;
        private Mode _oldMode = Mode.None;


        public float MoveSensitivity = 1.5f;
        //Cube移动前的位置
        private Vector3 origPosition;


        private Vector3 origZoom;
        private Vector3 resetPosition_;
        private Vector3 resetZoom_;
        private Quaternion resetRotation_;



        private void recordTransform() {

            resetPosition_ = this.transform.localPosition;
            resetZoom_ = this.transform.localScale;
            resetRotation_ = this.transform.localRotation;
        }
        public void Awake()
        {

            _mode = Mode.None;
            recordTransform();
            setupNone();

        }
        /// <summary>
        /// Gesture updates called by GestureInteractive
        /// </summary>
        /// <param name="startGesturePosition">The gesture origin position</param>
        /// <param name="currentGesturePosition">the current gesture position</param>
        /// <param name="startHeadOrigin">the origin of the camera when the gesture started</param>
        /// <param name="startHeadRay">the camera forward when the gesture started</param>
        /// <param name="gestureState">curent gesture state</param>
        public override void ManipulationUpdate(Vector3 startGesturePosition, Vector3 currentGesturePosition, Vector3 startHeadOrigin, Vector3 startHeadRay, GestureInteractive.GestureManipulationState gestureState)
        {
            base.ManipulationUpdate(startGesturePosition, currentGesturePosition, startHeadOrigin, startHeadRay, gestureState);
            if (gestureState == GestureInteractive.GestureManipulationState.Start) {
                origZoom = this.transform.localScale;
            }
            if (_mode == Mode.Rotation)
            {
                float x = currentGesturePosition.x - startGesturePosition.x;
                this.transform.Rotate(new Vector3(0, -x*2, 0));

            }
            else if (_mode == Mode.Zoom)
            {
                float y = currentGesturePosition.y - startGesturePosition.y;

                var zoom = origZoom + Vector3.one * y *2;
                if (zoom.x > 0.1f && zoom.y > 0.1f && zoom.z > 0.1f && zoom.x < 5f && zoom.y < 5f && zoom.z < 5f) {
                    this.transform.localScale = zoom;
                }
            }
        }

        internal void reset()
        {

            GDGeek.TweenLocalPosition.Begin(this.gameObject, 0.3f, resetPosition_);
            GDGeek.TweenScale.Begin(this.gameObject, 0.3f, resetZoom_);
            GDGeek.TweenRotation.Begin(this.gameObject, 0.3f, resetRotation_);
        }
        private void openGestureInteractive()
        {
            GestureInteractive gi = _object.GetComponent<GestureInteractive>();
            if (gi == null)
            {
                gi = _object.AddComponent<GestureInteractive>();
                if (gi.OnSelectEvents == null) {
                    gi.OnSelectEvents = new UnityEngine.Events.UnityEvent();
                }
                gi.OnSelectEvents.AddListener(delegate
                {
                    recordTransform();
                    UIManager.Instance.link(this);
                });
                gi.SetGestureControl(this);
            }
            gi.enabled = true;
      

        }
        private void closeGestureInteractive() {
            GestureInteractive gi = _object.GetComponent<GestureInteractive>();
            if (gi != null)
            {
                gi.enabled = false;
            }

        }

        private void openHandDraggable()
        {

            HoloToolkit.Unity.InputModule.HandDraggable hd = _object.GetComponent<HoloToolkit.Unity.InputModule.HandDraggable>();
            if (hd == null)
            {
                hd = _object.AddComponent<HoloToolkit.Unity.InputModule.HandDraggable>();
            }
            hd.RotationMode = HoloToolkit.Unity.InputModule.HandDraggable.RotationModeEnum.LockObjectRotation;
            hd.enabled = true;
            hd.HostTransform = this.transform;
        }
        private void closeHandDraggable()
        {

            HoloToolkit.Unity.InputModule.HandDraggable hd = _object.GetComponent<HoloToolkit.Unity.InputModule.HandDraggable>();
            if (hd != null)
            {
                hd.StopDragging();
                hd.OnFocusExit();
                hd.enabled = false;
            }
        }
        private void setupNone()
        {
            openGestureInteractive();
            closeHandDraggable();

        }
        private void setupDrag()
        {
            closeGestureInteractive();
            openHandDraggable();

        }
        private void setupRotation()
        {

            openGestureInteractive();
            closeHandDraggable();
        }
        private void setupZoom()
        {

            openGestureInteractive();
            closeHandDraggable();
        }
        protected override void Update() {
            base.Update();
            if (_oldMode != _mode) {
                switch (_mode)
                {
                    case Mode.None:
                        setupNone();
                        break;
                    case Mode.Drag:
                        setupDrag();
                        break;
                    case Mode.Rotation:
                        setupRotation();
                        break;
                    case Mode.Zoom:
                        setupZoom();
                        break;

                }
                _oldMode = _mode;
            }

        }

    }
}