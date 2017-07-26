using System;
using GDGeek;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using YiHe;
using HUX.Interaction;

public class LoadItem : MonoBehaviour, IInputClickHandler
{
    internal Task closeing()
    {
        Task task = new Task();
        TaskManager.PushFront(task, delegate
        {
            _text.gameObject.SetActive(false);
        });
        return task;
    }
    public TextMesh _text;
    private LoadData data_;

    internal Task loading(LoadData data)
    {
        Task task = new Task();

        TaskManager.PushFront(task, delegate
        {
            Debug.Log("Load :" + data._date);
            data_ = data;
            _text.text = data._date.ToString();
            _text.gameObject.SetActive(true);
        });
        return task;
    }
    static private bool isLoding_ = false;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (data_ != null && !isLoding_)
        {
            isLoding_ = true;
            GameObject.FindObjectOfType<AppBar>().GetComponent<AppBar>().BoundingBox = null;

            StartCoroutine(JsonGetter.Instance.getJson(GlobalManager.Instance.sceneGetUrl + "?id=" + data_.id, delegate (string json)
            {
                Debug.Log("json: " + json);
                HoloGeek.Snapshot.Root.Instance.deleteAllTarget();
                HoloGeek.Snapshot.Lens.DevelopPhoto(json);
                isLoding_ = false;
            }));
        }
        else
        {
            Debug.Log("data_ :" + data_);
        }
    }
}