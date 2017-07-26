using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GDGeek;

using UnityEngine.UI;

public class Logo :SpectatorView.SV_Singleton<Logo>
{


    public Task grow()
    {
        TweenTask tt = new TweenTask(delegate
        {
            return TweenScale.Begin(this.gameObject, 1.0f, Vector3.one);
        });
       
        return tt;
    }
    public Task reset()
    {

        TweenTask tt = new TweenTask(delegate
        {
            return TweenScale.Begin(this.gameObject, 0.1f, Vector3.one * 0.001f);
        });
        
        return tt;
    }
    public Text _text;
    public void setInfo(int score)
    {
        _text.text = "score:" + score.ToString();
    }
}
