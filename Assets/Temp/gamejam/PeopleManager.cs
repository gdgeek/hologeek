using GDGeek;
using HoloToolkit.Sharing;
using SpectatorView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : SpectatorView.SV_Singleton<PeopleManager>
{

    public Transform[] _maPos;
    public Transform[] _songPos;
    public bool _isRuning = false;
    public bool isSelf() {
        
        return (!HolographicCameraManager.Instance.localIPs.Contains(SpectatorViewManager.Instance.SpectatorViewIP.Trim()))
            && (!HolographicCameraManager.Instance.localIPs.Contains(SharingStage.Instance.ServerAddress.Trim()));
    }
        
    public Task grow()
    {
        TweenTask tt = new TweenTask(delegate
        {
            return TweenScale.Begin(this.gameObject, 1.0f, Vector3.one );
        });
        TaskManager.PushBack(tt, delegate
        {
            _isRuning = true;
           
            broadcast();
        });
        return tt;
    }
    public Task reset()
    {

        TaskSet ts = new TaskSet();
        TweenTask tt = new TweenTask(delegate
        {
            return TweenScale.Begin(this.gameObject, 0.1f, Vector3.one * 0.001f);
        });
        TaskManager.PushFront(tt, delegate
        {
            _isRuning = false;
            this.score = 0;
            maPos = 0;
            songPos = 0;
            time = 0f;
        });
        ts.push(tt);
        ts.push(Logo.Instance.reset());
        return ts;
    }


    void Start()
    {
        // Handles the ExplodeTarget message from the network.
        CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.Info] = this.OnInfo;
        


        // GetComponent<EnergyHubBase>().ResetAnimation();
        // AppStateManager.Instance.ResetStage();
        //  TaskManager.Run(PeopleManager.Instance.reset());
    }

    int maPos = 0;
    int songPos = 0;
    int score = 0;
    float time = 0;

    
  
    /// <summary>
    /// When a remote system has triggered an explosion, we'll be notified here.
    /// </summary>
    /// 

    public void OnInfo(NetworkInMessage msg) {
        if (!isSelf()) { 
            long userID = msg.ReadInt64();
            this.maPos = msg.ReadInt32();
            this.songPos = msg.ReadInt32();
            this.score = msg.ReadInt32();
            this.time = msg.ReadInt32();
            refresh();
        }

    }
    public void onHit(string id) {
        if (id == "ma")
        {
            maPos++;
        }
        else if (id == "song") {

            songPos++;
        }

        HoloDebug.Log("id:" + id);

        HoloDebug.Log("mapos:" + maPos);
        HoloDebug.Log("songPos:" + songPos);
        this.score++;
        refresh();
        broadcast();
      
    }
    public void broadcast() {
        if (isSelf())
        {
            allTime = 0;
            CustomMessages.Instance.SendInfo(this.maPos, this.songPos, this.score, Mathf.FloorToInt(this.time));
        }
    }
    public void refresh()
    {
        Transform map = this._maPos[this.maPos % this._maPos.Length];
        Transform songp = this._songPos[this.songPos % this._songPos.Length];
        People[] peoples = this.gameObject.GetComponentsInChildren<People>();
        for (int i = 0; i < peoples.Length; ++i) {
            if (peoples[i]._id == "song") {
                HoloDebug.Log("sp:" + songp.position);
                peoples[i].transform.SetParent(songp);
            }
            else if (peoples[i]._id == "ma")
            {
                HoloDebug.Log("mp:" + map.position);
                peoples[i].transform.SetParent(map);
            }

            peoples[i].transform.localPosition = Vector3.zero;
        }
        if (45 - Mathf.FloorToInt(this.time) <= 0)
        {
            Score.Instance.setInfo(0, this.score);
            Logo.Instance.setInfo(this.score);
            TaskManager.Run(Logo.Instance.grow());
            this._isRuning = false;
        }
        else {
            Score.Instance.setInfo(45 - Mathf.FloorToInt(this.time), this.score);
        }
        
    }
    float allTime = 0;
    // Update is called once per frame
    void Update () {
        if (_isRuning) {
            if (isSelf())
            {
                int old = Mathf.FloorToInt(time);
                time += Time.deltaTime;
                if (old != Mathf.FloorToInt(time))
                {
                    broadcast();
                    refresh();
                }
            }
            else
            {
                allTime += Time.deltaTime;
                if (allTime >= 0.5)
                {
                    if (isSelf())
                    {
                        broadcast();
                    }
                    else
                    {

                        allTime = 0;
                    }
                }
            }
        }
    }
}
