using System;
using System.Collections;
using System.Collections.Generic;
using GDGeek;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using YiHe;

public class SaveItem : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    public Action OnSaveComplete;
    internal Task closeing()
    {
        Task task = new Task();
        TaskManager.PushFront(task, delegate
        {
            if (_add != null)
            {
                _add.SetActive(false);
            }
            _text.gameObject.SetActive(false);
        });
        return task;
    }
    public TextMesh _text;
    public GameObject _add;
    private SaveData data_ = null;

    internal Task loading(SaveData data)
    {
        Task task = new Task();

        TaskManager.PushFront(task, delegate
        {
            data_ = data;
            if (data._isset)
            {
               
                _text.text = data._date.ToString();
                _text.gameObject.SetActive(true);
                if (_add != null)
                {
                    _add.SetActive(false);
                }
            }
            else
            {
                if (_add != null)
                {
                    _add.SetActive(true);
                }
                _text.gameObject.SetActive(false);
            }
        });
        return task;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        string json = HoloGeek.Snapshot.Lens.TakePhoto();
        Debug.Log(data_);
        StartCoroutine(WebUploader.Instance.uploadScene(GlobalManager.Instance.sceneUploadUrl, data_.id, TimeUtility.ConvertDateTimeInt(DateTime.Now), json, OnSaveComplete));
     
    }
}
