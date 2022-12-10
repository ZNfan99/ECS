using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcObj : ObjectBase
{
    public NPC_Info m_info;

    public NpcObj(int plot, ObjectInfo info)
    {
        m_info = new NPC_Info(plot, info);
        m_insID = info.ID;
        m_modelPath = info.m_res;

    }
    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
        m_go.name = m_info.m_name; 
    }

    public override void OnCreate()
    {
        base.OnCreate();
        //加组件 拥有的功能
    }
}
