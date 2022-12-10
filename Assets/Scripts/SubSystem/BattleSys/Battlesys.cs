using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Battlesys : UIbase
{
    private Button m_gatherBtn;
    private Slider m_gatherSlider;
    private int m_gatherInsid;
    private UpDateUtils m_updateUtils;
    private float m_timer;
    private TimeTool timetool;

    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        MsgCenter.GetT().AddListener("ClientMsg", RefreshBtn);

        MsgCenter.GetT().AddListener("ServerMsgGather", ServerNotify);
    }

    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            m_gatherSlider.gameObject.SetActive(true);
            Debug.Log("开始采集");
            timetool = GameObject.Find("UIRoot").GetComponent<TimeTool>();
            m_updateUtils = m_go.transform.AddComponent<UpDateUtils>();
            m_updateUtils.m_call = SetValue;
            timetool.TimeInvoke(DateTime.Now.Ticks / 10000000, 4, DelCall);
        }
    }

    private void DelCall()
    {
        m_timer = 0;
        m_gatherSlider.value = 0; 
        GameObject.Destroy(m_go.transform.GetComponent<UpDateUtils>());
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }

    private void SetValue()
    {
        m_timer += Time.deltaTime;
        m_gatherSlider.value = m_timer / 3;
    }

    public override void DoShow(bool active)
    {
        base.DoShow(active);
        m_gatherBtn = m_go.transform.Find("gather_btn").GetComponent<Button>();
        m_gatherBtn.onClick.AddListener(() => {
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("gather", 1);
            MsgCenter.GetT().SendMsg("ServerMsg", notify);
        });
        m_gatherSlider = m_go.transform.Find("gather_slider").GetComponent<Slider>();
        m_gatherBtn.gameObject.SetActive(false);
        m_gatherSlider.gameObject.SetActive(false);
    }

    public override void Destory()
    {
        base.Destory();
        MsgCenter.GetT().RemoveListener("ClientMsg", RefreshBtn);
    }


    private void RefreshBtn(Notification notiy)
    {
        if (notiy.msg.Equals("gather_trigger"))
        {
            m_gatherInsid = (int)notiy.data[0];
            m_gatherBtn.gameObject.SetActive(true);
        }
    }
}
