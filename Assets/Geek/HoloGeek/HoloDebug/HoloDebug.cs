using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoloDebug : HoloToolkit.Unity.Singleton<HoloDebug>
{

    
    private Queue<string> texts_ = new Queue<string>();
    public Text _text;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < list_.Count; ++i) {
            HoloDebug.Log(list_[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.log("abcdefg");
        }
	}
    private int n = 0;
    private void refresh() {
        if (texts_.Count > 30) {
            texts_.Dequeue();
        }
        string[] texts = texts_.ToArray();
        string log = "";
        for (int i = texts.Length-1; i >=0 ; --i) {
            log +=texts[i] + "\n";
        }
        _text.text = log;
    }
    private static List<string> list_ = new List<string>();
    public static void Log(string text)
    {
        if (HoloDebug.Instance != null)
        {
            HoloDebug.Instance.log(text);
        }
        else {
            list_.Add(text);
        }
    }
    public void log(string text)
    {
        texts_.Enqueue("" + (n++) + ":" + text);
        refresh();
    }
}
