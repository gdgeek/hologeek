using UnityEngine;
using System.Collections;
using System;

public static class TimeUtility
{

    public static DateTime FromTimeStamp(string timeStamp)
    {
        DateTime dtStart = new DateTime(1970, 1, 1);
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public static int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = new System.DateTime(1970, 1, 1);
        return (int)(time - startTime).TotalSeconds;
    }
}
