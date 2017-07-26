using GDGeek;
using HUX.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxManager : Singleton<MessageBoxManager> {
    public GameObject messageBoxPrefab;
    public GameObject diaglogPrefab;
    public MessageBox GetMessageBox(Transform parent)
    {
        var msgbox = Instantiate(messageBoxPrefab, parent);
        msgbox.SetActive(true);
        return msgbox.GetComponent<MessageBox>();
    }

    public IEnumerator LaunchDialog(string title, string message, Action<SimpleDialogResult> over)
    {
        var buttons = SimpleDialog.ButtonTypeEnum.Yes;
        //var dialogObj = (GameObject)Resources.Load("MRDesignLab/HUX/Prefabs/Dialogs/SimpleDialogShell");
        var dialogObj = MessageBoxManager.Instance.diaglogPrefab;
        SimpleDialog dialog = SimpleDialog.Open(dialogObj, buttons, title, message);
        dialog.OnClosed += over;
        while (dialog.State != SimpleDialog.StateEnum.Closed)
        {
            yield return null;
        }
    }
}
