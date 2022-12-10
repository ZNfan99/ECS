using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TaskSys : UIbase
{
    private Text taskText;
    private Button acceptBtn;
    public override void DoCreate(string path)
    {
        base.DoCreate(path);
        MsgCenter.GetT().AddListener("ServerMsgTask", RefreshBtn);
    }

    private void RefreshBtn(Notification obj)
    {
        if (obj.msg.Equals("Task_callback"))
        {
            Debug.Log("任务领取成功");
        }
    }

    public override void DoShow(bool active)
    {
        base.DoShow(active);
        taskText = m_go.transform.Find("TaskText").GetComponent<Text>();
        acceptBtn = m_go.transform.Find("AcceptButton").GetComponent<Button>();

        taskText.text = GameData.GetT().GetTaskDataById(1).taskName;

        acceptBtn.onClick.AddListener(() => {
            //交互服务器
            Notification notify = new Notification();
            notify.Refresh("AcceptTask", 1);
            MsgCenter.GetT().SendMsg("ServerMsg", notify);
        });

    }
}
