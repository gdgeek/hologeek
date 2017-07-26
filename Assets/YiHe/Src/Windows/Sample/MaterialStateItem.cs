using UnityEngine;
using System.Collections;
namespace YiHe { 
    public class MaterialStateItem : StateItem
    {
        public Material PressedMat;
        private Material originMat;
        private MeshRenderer _renderer;
  

        protected override void Awake()
        {
            base.Awake();
            _renderer = transform.Find("UIButtonSquare/UIButtonSquareFace").GetComponent<MeshRenderer>();
            originMat = _renderer.material;
        }
        protected override void press()
        {
           // base.press();
            objBoxCollider_.enabled = false;
            _renderer.material = PressedMat;
        }

        protected override void unpress()
        {
           // base.unPress();
            objBoxCollider_.enabled = true;
            _renderer.material = originMat;
        }
    }
}