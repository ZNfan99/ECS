using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTest : MonoBehaviour
{
    TimeTool times;
    long startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = DateTime.Now.Ticks / 10000000;
        times = GameObject.Find("TimeTool").GetComponent<TimeTool>();
        times.TimeInvoke(startTime, 3, () =>
         {
             Debug.Log("—”≥Ÿ3√Î");
         });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
