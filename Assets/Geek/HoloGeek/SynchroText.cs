using System;
using System.Collections;
using System.Collections.Generic;
using HoloGeek;
using HoloToolkit.Sharing;
using UnityEngine;

public class SynchroText : HoloGeek.SynchroData
{

    public TextMesh _text;
    private string _oldText;

    class Handler : IMessageReader, IMessageWriter
    {
        private SynchroText st_;
        public Handler(SynchroText text) {
            st_ = text;
        }
        public void readFrom(NetworkInMessage msg)
        {
            st_._text.text = msg.ReadString().GetString();
        }

        public void writeTo(NetworkOutMessage msg)
        {
            msg.Write(new XString(st_._text.text));
        }
    }
    public override bool dirty()
    {
        if (_oldText != _text.text) {
            return true;
        }
        return false;
    }

    public override IMessageReader getReader()
    {
        return new Handler(this);
    }

    public override IMessageWriter getWriter()
    {
        return new Handler(this);
    }

    public override void sweep()
    {
        _oldText = _text.text;
    }

  
}
