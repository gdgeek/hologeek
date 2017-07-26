using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只用一次的点击放置功能
/// </summary>
public class TapToPlaceOnce : MonoBehaviour {

    private HoloToolkit.Unity.InputModule.TapToPlace ttp_;
    /// <summary>
    /// 开始的时候回调
    /// </summary>
    public Action onBegin;

    /// <summary>
    /// 结束的时候回调
    /// </summary>
    public Action onEnd;
    public Collider _collider;
    void Start () {
        _collider = this.gameObject.GetComponent<Collider>();
        ttp_ = this.gameObject.GetComponent<HoloToolkit.Unity.InputModule.TapToPlace>();
        
        if (ttp_ == null)
        {
            ttp_ = this.gameObject.AddComponent<HoloToolkit.Unity.InputModule.TapToPlace>();
        }

        if (ttp_.IsBeingPlaced == false)
        {
            ttp_.IsBeingPlaced = true;
        }
        if (onBegin != null) {
           onBegin();
        }
        ttp_.DefaultGazeDistance = 10f;
        ttp_.AllowMeshVisualizationControl = false;
       // _collider.enabled = false;
    }
    private void Update()
    {
        if (ttp_ != null && ttp_.IsBeingPlaced == false)
        {

            GameObject.Destroy(ttp_);
            HoloToolkit.Unity.Interpolator ip = this.gameObject.GetComponent<HoloToolkit.Unity.Interpolator>();
            if (ip != null)
            {
                GameObject.Destroy(ip);
            }

            if (onEnd != null)
            {
                onEnd();
            }

            //_collider.enabled = true;
            Destroy(this);
        }
    }
}
