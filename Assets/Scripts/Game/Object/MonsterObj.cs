using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : ObjectBase
{
    public Monster_Info m_info;

    public Monster(MonsterType type, Monster_Info info)
    {
        info.m_type = type;
        m_info = info;
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void OnCreate()
    {
        base.OnCreate();
        m_go.name = m_info.m_name;
    }
}

//正常怪物
public class Normal : Monster
{
    public Normal(Monster_Info info)
        : base(MonsterType.Normal, info)
    {
    }

    public Normal(ObjectInfo info) : base(MonsterType.Normal, new Monster_Info(MonsterType.Normal, info))
    {
    }

    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }

    public override void OnCreate()
    {
        base.OnCreate();

        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.SetData(m_info.m_name);
        m_pate.m_name.gameObject.SetActive(true);
        m_pate.m_hp.gameObject.SetActive(true);
        m_pate.m_mp.gameObject.SetActive(true);
        m_pate.m_gather.gameObject.SetActive(false);
    }
}
//采集物
public class Gather : Monster
{
    public Gather(Monster_Info info)
        : base(MonsterType.Gather, info)
    {
    }
    public Gather(ObjectInfo info) :
        base(MonsterType.Gather, new Monster_Info(MonsterType.Gather, info))
    {
    }

    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }

    public override void OnCreate()
    {
        base.OnCreate();

        StaticCircleCheck check = m_go.AddComponent<StaticCircleCheck>();
        check.m_taget = World.GetT().m_plyer.m_go;
        check.m_call = (isenter) =>
        {
            Debug.Log(string.Format("主角触发了我,我是{0}", m_info.m_res));
            Notification notify = new Notification();
            notify.Refresh("gather_trigger", m_info.ID);
            MsgCenter.GetT().SendMsg("ClientMsg", notify);
        };

        MsgCenter.GetT().AddListener("ServerMsg", ServerNotify);
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();

        m_pate.m_name.gameObject.SetActive(false);
        m_pate.m_hp.gameObject.SetActive(false);
        m_pate.m_mp.gameObject.SetActive(false);
        m_pate.m_gather.gameObject.SetActive(true);
    }
    private void ServerNotify(Notification obj)
    {
        if (obj.msg.Equals("gather_callback"))
        {
            int insID = (int)obj.data[0];
            m_pate.SetData((int)obj.data[1]);
        }
    }
}