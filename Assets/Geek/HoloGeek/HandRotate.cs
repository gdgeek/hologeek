// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System;

namespace HoloGeek
{


    public class HandRotate : MonoBehaviour, HoloToolkit.Unity.InputModule.IManipulationHandler
    {

        public Transform _transform;
        public float _speed = 4.0f;
        public void OnManipulationStarted(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {
        }
        public void Start()
        {
            HoloToolkit.Unity.InputModule.InputManager.Instance.PushModalInputHandler(gameObject);
        }
        public void OnManipulationUpdated(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {

            float rotationFactor = eventData.CumulativeDelta.x * 0.5f;
            _transform.Rotate(new Vector3(0, -1 * rotationFactor * _speed, 0));
        }
        public void OnManipulationCompleted(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {

        }
        public void OnManipulationCanceled(HoloToolkit.Unity.InputModule.ManipulationEventData eventData)
        {

        }
    }

}
