using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData
{
    public long startTimes;
    public float endTimes;
    public Action func;
    public float currentTimes;

    public TimeData(long startTimes, float endTimes, Action func, float currentTimes)
    {
        this.startTimes = startTimes;
        this.endTimes = endTimes;
        this.func = func;
        this.currentTimes = currentTimes;
    }
}

/// <summary>
/// ¼ÆÊ±Æ÷
/// </summary>
public class TimeTool : MonoBehaviour
{
    List<TimeData> timeToolList = new List<TimeData>();
    public void TimeInvoke(long startTime,float delayTime, Action func)
    {
        TimeData timeData = new TimeData(startTime, delayTime, func, 0);
        timeToolList.Add(timeData);
    }

    public void RemoveTimeInvoke(long startTime)
    {
        for (int i = timeToolList.Count - 1; i >= 0; i--)
        {
            if (timeToolList[i].startTimes == startTime)
            {
                timeToolList.RemoveAt(i);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToolList.Count > 0)
        {
            for (int i = timeToolList.Count - 1; i >= 0; i--)
            {
                timeToolList[i].currentTimes = DateTime.Now.Ticks / 10000000 - timeToolList[i].startTimes;
                if (timeToolList[i].currentTimes - timeToolList[i].endTimes >= 0)
                {
                    timeToolList[i].func?.Invoke();
                    timeToolList.RemoveAt(i);
                }
            }
        }
    }
}
