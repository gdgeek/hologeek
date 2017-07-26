using HoloToolkit.Sharing.SyncModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloGeek;

public class TextTest : HoloGeek.SynchroData
{
   
    public TextMesh _text;
    [Serializable]
    class Data {
        [SerializeField]
        public string text;
    }
    /*
    public override void fromJson(string json)
    {
        Data data = JsonUtility.FromJson<Data>(json);
        _text.text = data.text;

        
    }

    public override string toJson()
    {
        Data data = new Data();
        data.text = _text.text;
        return JsonUtility.ToJson(data);
    }*/

    public override IMessageReader getReader()
    {
        throw new NotImplementedException();
    }

    public override IMessageWriter getWriter()
    {
        throw new NotImplementedException();
    }

    public override bool dirty()
    {
        throw new NotImplementedException();
    }

    public override void sweep()
    {
        throw new NotImplementedException();
    }
}
