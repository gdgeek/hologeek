using GDGeek;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using HoloGeek;

namespace GDGeek { 
    public class GameLogic : MonoBehaviour
    {
        public string _logoScene;
        public string _mainScene;
        private FSM fsm_ = new FSM();
	    // Use this for initialization
	    void Start () {
            fsm_.addState("begin", beginState());
            fsm_.addState("logo", logoState(), "begin");
            fsm_.addState("opation", opationState(), "begin");
            fsm_.addState("select", selectState(), "begin");
            fsm_.addState("sample", sampleState());
            fsm_.init("begin");
		
	    }

        private StateBase opationState()
        {
            return new State();
        }

        private StateBase selectState()
        {
            return new State();
        }

        private StateBase logoState()
        {
            return new State();
        }

        private StateBase sampleState()
        {
            AsyncOperation begin = null;
            State state = TaskState.Create(delegate {
                TaskList tl = new TaskList();

                Task load = new Task();
                TaskManager.PushFront(load, delegate
                {
                    begin = SceneManager.LoadSceneAsync(_mainScene, LoadSceneMode.Additive);
                    /*SceneManager.sceneLoaded += delegate (Scene scene, LoadSceneMode mode)
                    {
                       
                        Debug.Log("!!!!!" + scene.name);
                    };
                    */
                   
                });
                TaskManager.AddAndIsOver(load, delegate
                {

                    Debug.Log("!!!!!？？？？？？？");
                    return begin.isDone;
                });
                tl.push(load);
                TaskManager.PushBack(load, delegate
                {
                   
                    var sample = SceneManager.GetSceneByName(_mainScene);
                    SceneManager.SetActiveScene(sample);
              
                    
                });
                return tl;
              
            }, fsm_, "");

         
            state.onOver += delegate
            {
                SceneManager.UnloadSceneAsync(_mainScene);
            };
            return state;
        }

        private StateBase beginState()
        {
            AsyncOperation begin = null;
            State state = TaskState.Create(delegate
            {
                Task task = new Task();

                TaskManager.AddAndIsOver(task, delegate {
                    return begin != null && begin.isDone;
                });

                TaskManager.AddAndIsOver(task, delegate {
                 //   Debug.Log(ImportExportAnchorManager.Instance.currentState);
                    return ImportExportAnchorManager.Instance != null && ImportExportAnchorManager.Instance.currentState == ImportExportAnchorManager.ImportExportState.Ready;
                });

                return task;
            }, this.fsm_, "sample");
           
            state.onStart += delegate
            {
                begin = SceneManager.LoadSceneAsync(_logoScene, LoadSceneMode.Additive);
               
            };
            state.onOver += delegate
            {
                SceneManager.UnloadSceneAsync(_logoScene);
            };
            return state;
        }

    }
}
