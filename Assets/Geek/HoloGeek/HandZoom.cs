// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System;

namespace HoloGeek
{


    public class HandZoom : MonoBehaviour, HoloToolkit.Unity.InputModule.IManipulationHandler
    {


        public float _max = 3.0f;
        public float _min = 0.1f;
        public Transform _transform;
        public float _speed = 4.0f;
        public float _zoom = 1.0f;
        public void OnManipulationStarted(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {
            HoloToolkit.Unity.InputModule.InputManager.Instance.PushModalInputHandler(gameObject);
        }
        public void Start()
        {
        }
        public void OnManipulationUpdated(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {
           
            float rotationFactor = eventData.CumulativeDelta.y * 0.5f;
            if (rotationFactor >= _speed) {
                rotationFactor = _speed;
            }
            if (rotationFactor > 0f)
            {
                _zoom += Time.deltaTime * 0.3f;
            }
            else if (rotationFactor < -0)
            {
                _zoom -= Time.deltaTime * 0.3f;
            }
             

            if (_zoom < _min)
            {
                _zoom = _min;
            }
            if (_zoom > _max)
            {
                _zoom = _max;
            }
            _transform.localScale = (Vector3.one * _zoom);
        }
        public void OnManipulationCompleted(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {
            Debug.Log("w");
            HoloToolkit.Unity.InputModule.InputManager.Instance.ClearFallbackInputStack();
        }
        public void OnManipulationCanceled(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {

            Debug.Log("t");
            HoloToolkit.Unity.InputModule.InputManager.Instance.ClearFallbackInputStack();
        }
    }

}
