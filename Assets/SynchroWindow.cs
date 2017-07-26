using HoloGeek;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using HoloToolkit.Sharing;
using HoloGeek.Net;

namespace YiHe {
    /// <summary>
    /// 网络同步窗口信息。
    /// </summary>
    /// <seealso cref="HoloGeek.SynchroData" />
    public class SynchroWindow : SynchroData {
        public WindowsManager _manager;
        WindowsManager.Data data_ = new WindowsManager.Data();
      
        public override bool dirty()
        {
            if (data_.hide != _manager._data.hide ||
                 data_.window != _manager._data.window ||
                 data_.ingoreCast != _manager._data.ingoreCast||
                 data_.page != _manager._data.page
                 ) {
                return true;
            }
            return false;
        }

        public override IMessageReader getReader()
        {
            MessageReader reader = new MessageReader();
            reader.onReadFrom += delegate (NetworkInMessage msg)
            {
                _manager._data.window = msg.ReadInt32();
                _manager._data.page = msg.ReadInt32();
                _manager._data.ingoreCast = (msg.ReadByte() != 0);
                _manager._data.hide = (msg.ReadByte() != 0);
            };
            return reader;
        }

        public override IMessageWriter getWriter()
        {
            MessageWriter writer = new MessageWriter();
            writer.onWriteTo += delegate (NetworkOutMessage msg)
            {
                msg.Write((int)_manager._data.window);
                msg.Write((int)_manager._data.page);
                msg.Write((byte)(_manager._data.ingoreCast ? 1 : 0));
                msg.Write((byte)(_manager._data.hide ? 1 : 0));
               
            };
            return writer;
        }

        public override void sweep()
        {
            data_.hide = _manager._data.hide;
            data_.window = _manager._data.window;
            data_.ingoreCast = _manager._data.ingoreCast;
            data_.page = _manager._data.page;
            _manager.refresh();
        }

      
    }
}
