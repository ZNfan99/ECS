using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class LocalProps
{
    public static Dictionary<long, SPlayer> players = new Dictionary<long, SPlayer>();


}

public class ServerInit : MonoBehaviour
{
    public Vector3 m_playerPos;
    public Dictionary<int, Vector3> m_otherPosDic;

    private void Awake()
    {
        MsgCenter.GetT().AddListener("MovePos", (notify) =>
        {
            if (notify.msg.Equals("Player"))
            {
                m_playerPos = (Vector3)notify.data[0];
            }
            else if (notify.msg.Equals("Other"))
            {
                if (m_otherPosDic == null)
                {
                    m_otherPosDic = new Dictionary<int, Vector3>();
                }
                int insid = (int)notify.data[0];
                Vector3 pos = (Vector3)notify.data[1];
                if (!m_otherPosDic.ContainsKey(insid))
                {
                    m_otherPosDic.Add(insid, pos);
                }
                else
                {
                    m_otherPosDic[insid] = pos;
                }
            }

        });

        MsgCenter.GetT().AddListener("ServerMsg", (notify) =>
        {
            if (notify.msg.Equals("gather"))
            {
                Debug.Log($"点击采集按钮  Insid：{(int)notify.data[0]}");
                int insid = (int)notify.data[0];

                notify.Refresh("gather_callback", insid,2);//采集回复消息，采集ID，采集数量
                MsgCenter.GetT().SendMsg("ServerMsgGather", notify);

            }
            if (notify.msg.Equals("AcceptTask"))
            {
                Debug.Log($"点击任务按钮  Insid：{(int)notify.data[0]}");
                int taskid = (int)notify.data[0];
                notify.Refresh("Task_callback", taskid, 2);
                MsgCenter.GetT().SendMsg("ServerMsgTask", notify);
            }
        });

        //任务配置表解析 S端



        //解析配置表 初始化角色信息
        SPlayer splayer = new SPlayer();
        splayer.InitPlayer();
        splayer.m_insid = 1;
        splayer.Hp = 100;

        splayer.components.Add(ComponentType.battle,new BattleComponent());
        //splayer.components.Add(ComponentType.task, new TaskComponent());

        LocalProps.players.Add(splayer.m_insid, splayer);
        // 初始化 完所有角色，给所有角色相关组件注册回调
        if (LocalProps.players == null) return;
        foreach (var item in LocalProps.players)
        {
            foreach (var item1 in item.Value.components)
            {
                item1.Value.GetPlayerById = GetPlayer;
                item1.Value.Init();
            }
        }
    }

    private SPlayer GetPlayer(long id)
    {
        using (var tmp = LocalProps.players.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (tmp.Current.Key == id)
                {
                    return tmp.Current.Value;
                }
            }
        }
        return null;
    }

}

public enum ComponentType:byte
{
    nil =0,
    task,
    battle,

}

public class SkillProp
{
   public float range;
    //范围、攻击力、时长、触发节点
}

public class SPlayer
{
    public long m_insid;

    public Vector3 m_pos;
    public float Hp;
    public float Mp;
    public float Atk;
    //
    public List<int> buffs;
    public List<SkillProp> skills;

    public Dictionary<ComponentType, SComponent> components;


    public void InitPlayer()
    {
        buffs = new List<int>();
        skills = new List<SkillProp>();
        components = new Dictionary<ComponentType, SComponent>();
    }

    public void PropOperation(int type, float value)
    {
        switch (type)
        {
            case 1:
                Hp += value;
                break;
            case 2:
                Mp += value;
                break;
        }
        Notification m_notify = new Notification();
        m_notify.Refresh("ByServer", type,value);
        MsgCenter.GetT().SendMsg("propchange", m_notify);
    }


}

public class SComponent
{
   public Func<long, SPlayer> GetPlayerById;

    Notification m_notify;
    public virtual void S2CMsg(string cmd,object value)
    {
        if (m_notify == null)
        {
            m_notify = new Notification();
        }
        m_notify.Refresh("ByServer",value);
        MsgCenter.GetT().SendMsg(cmd, m_notify);
    }

    public virtual void Init()
    { 

    }
}

public class TaskComponent: SComponent
{
    public List<int> Tasks;

    public override void Init()
    {
        //======侦听服务器采集操作  处理任务逻辑  GatherAction
        MsgCenter.GetT().AddListener("GatherAction", (notify) =>
        {
            //Debug.Log("点击任务");
        });
    }
}

public class BattleComponent: SComponent
{
    public override void Init()
    {
        MsgCenter.GetT().AddListener("ByClent_Battle", (notify) => {
            if (notify.msg.Equals("atkOther"))
            {
                int atkId = (int)notify.data[0];
                int targetID = (int)notify.data[1];
                int skillID = (int)notify.data[2];

                Debug.Log("攻击");
            }
           
        });
    }
}
