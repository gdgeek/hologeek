using GDGeek;
using HUX.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace YiHe
{
    public class WindowsManager : HUX.Receivers.InteractionReceiver
    {
        [Serializable]
        public class WindowPair {
            public GameObject button;
            public WindowBase window;

        }
        public WindowPair[] _windows;
        public IngoreCastItem _ingore;
        [Serializable]
        public class Data {
            public int page;
            public int window = 0;
            public bool ingoreCast = false;
            public bool hide = false;
        };
     
        public Data _data = new Data();

      



     


        public TextMesh _text;
        public GameObject _upPage;
        public GameObject _downPage;
        

        public HoloToolkit.Unity.SimpleTagalong _tagalong;
        private WindowBase _curr = null;
        public HideItem _hide;
        public ButtonGroupManager _tapScript;
        private bool _refreshUpdate = true;

        private void Update()
        {
            if (_refreshUpdate) {
                this._refreshData();
            }
          
        }

        

        Task changeWindowTask(WindowBase window)
        {
            _curr = window;

            TaskList tl = new TaskList();

            Task loading = _curr.loading();
            TaskManager.PushFront(loading, delegate
            {
                this.refreshWindow();
            });

            tl.push(loading);

            Task refresh = _curr.refresh();
            TaskManager.PushFront(refresh, delegate
            {
                this.refreshPageButton();
            });
            TaskManager.PushFront(refresh, delegate
            {
                _curr.gameObject.SetActive(true);
            }); 
            tl.push(refresh);
            return tl;

         

        }

     

        void refreshWindow() {
            _text.text = _curr._title;
            for (int i = 0; i < _windows.Length; ++i) {
                var windows = _windows[i];
                if (i == _data.window)
                {
                    _curr.gameObject.SetActive(true);
                }
                else {
                    _curr.gameObject.SetActive(false);
                }
            }
          
        }

        public void Start()
        {
            refresh();
        }
        protected override void OnTapped(GameObject obj, InteractionManager.InteractionEventArgs eventArgs)
        {
            
            if (_curr.isBusy) {
                return;
            }

            switch (obj.name)
            {
                case "UpPage":
                    pageTurning(-1);
                    break;
                case "DownPage":
                    pageTurning(1);
                    break;
                case "IngoreCast":

                    setIngoreCast(!this._data.ingoreCast);
                    break;
                case "Hide":
                    setHideInfo(!this._data.hide);
                    break;
                case "Save":
                case "Load":
                case "Sample":
                    changeWindow(obj);
                    break;

            }
         

            base.OnTapped(obj, eventArgs);
        }
        private void changeWindow(int window) {
            _data.window = window;
            refresh();
        }
        private void changeWindow(GameObject obj)
        {
            _data.window = this.getWindowN(obj);
            refresh();
        }

      
        private void setHideInfo(bool hide)
        {
            _data.hide = hide;
            refresh();
           
        }

        private void setIngoreCast(bool ingore)
        {
            this._data.ingoreCast = ingore;
            refresh();

        }

        private void pageTurning(int turn)
        {
            if (this._data.page + turn >= 0 && this._data.page + turn < _curr.pages)
            {
                this._data.page = this._data.page + turn;
                refresh();
            }
        }
        public void refresh()
        {
            _refreshUpdate = true;
        }
        private void _refreshData()
        {
            if (_curr != null && _curr.isBusy) {
              
                return;
            }

            WindowPair pair = this.getWindowPair(_data.window);
            if (_curr != pair.window)
            {
                _tapScript.switchToThisItem(pair.button);
                TaskManager.Run(changeWindowTask(pair.window));
                _curr = pair.window;
                return;
            }

            if (this._data.page != _curr.page) {
                _curr.page = this._data.page;
                Task refresh = _curr.refresh();
                TaskManager.PushFront(refresh, delegate
                {
                    this.refreshPageButton();
                });
                TaskManager.Run(refresh);
                return;
            }
            if (this._data.ingoreCast != this._ingore.pressed) {

                var masks =  HoloToolkit.Unity.InputModule.GazeManager.Instance.RaycastLayerMasks;
                if (this._data.ingoreCast)
                {
                    var mask = HUX.Focus.FocusManager.Instance.RaycastLayerMask;
                    int wall = LayerMask.GetMask("Wall");
                    LayerMask nowall = mask & ~wall;
                    HUX.Focus.FocusManager.Instance.RaycastLayerMask = nowall;
                    HoloToolkit.Unity.InputModule.GazeManager.Instance.RaycastLayerMasks = new LayerMask[]{ nowall };
                }
                else {

                    var mask = HUX.Focus.FocusManager.Instance.RaycastLayerMask;
                    int wall = LayerMask.GetMask("Wall");
                    LayerMask haswall = mask | wall;
                    HUX.Focus.FocusManager.Instance.RaycastLayerMask = haswall;
                    HoloToolkit.Unity.InputModule.GazeManager.Instance.RaycastLayerMasks = new LayerMask[] { haswall };
                }
                this._ingore.pressed = this._data.ingoreCast;
                return;
            }


            if (_hide.pressed != _data.hide) {
                CloneObjInfoManager.Instance.setHide(_data.hide);
                _hide.pressed = _data.hide;
            }

        
            _refreshUpdate = false;
        }
        private int getWindowN(GameObject obj)
        {
            for (int i = 0; i < this._windows.Length; ++i) {
                var pair = _windows[i];
                if (pair.button == obj) {
                    return i;
                }
            }
            return -1;
        }

        private WindowPair getWindowPair(int window)
        {
            
            var pair = _windows[window];
            return pair;
            
        }

       
        void refreshPageButton()
        {

            if (_curr.pages > 1)
            {
                _text.text = _curr._title + "(第" + (_curr.page + 1) + "/" + _curr.pages + "页)";
            }
            else
            {

                _text.text = "";
            }


            if (_curr.page == 0)
            {
                _upPage.SetActive(false);
            }
            else
            {
                _upPage.SetActive(true);
            }

            if (_curr.page + 1 >= _curr.pages)
            {
                _downPage.SetActive(false);
            }
            else
            {
                _downPage.SetActive(true);
            }
        }
    }

}

