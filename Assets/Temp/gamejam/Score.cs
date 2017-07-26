using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : SpectatorView.SV_Singleton<Score>
{
    public Text _text;
    public void setInfo(int time, int score) {
        _text.text = "time:" + time.ToString() + ";score" + score.ToString();
    }
}
