using GDGeek;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public class ForShare : MonoBehaviour {

	// Use this for initialization
	void Start () {
      /*  Debug.Log(getNewIPAddress(3));

        GDGeek.TaskCircle tc = new GDGeek.TaskCircle();
        int n = 0;
        tc.push(new TaskPack(delegate
        {
            Task t = new TaskWait(0.1f);

            TaskManager.PushFront(t, delegate
            {
                string ip = this.getNewIPAddress(n);
                Debug.Log(ip);
               HoloToolkit.Sharing.SharingStage.Instance.ConnectToServer(ip, 20602);
            });


            TaskManager.PushBack(t, delegate
            {
                n++;
                Debug.Log(HoloToolkit.Sharing.SharingStage.Instance.IsConnected);
                Debug.Log(n);
                if (n > 255) {
                    tc.forceQuit();
                }
            });
            return t;

        }));

        TaskManager.Run(tc);
      
      */
        //Debug.Log();

    }
   /* public string getNewIPAddress(int n) {
        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                string pat = @"^([0-9]+\.[0-9]+\.[0-9]+\.)[0-9]+$";

                Regex r = new Regex(pat, RegexOptions.IgnoreCase);
                Match m = r.Match(ip.ToString().Trim());
                if (m.Success)
                {
                    Debug.Log(2);
                    if (m.Groups.Count >= 2)
                    {
                        Debug.Log(3);
                        return m.Groups[1] + n.ToString();
                    }

                }
            }
        }
     
        return null;
    }*/
	
}
