using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据的基类（所有实例的数据）
/// </summary>
public class ObjectInfo
{
    public int ID;
    public string m_name;
    public Vector3 m_pos;
    public string m_res;
    public MonsterType m_type;
}

/// <summary>
/// 玩家的数据
/// </summary>
public class Player_Info : ObjectInfo
{
    public int m_level;
    public float m_HP;
    public float m_MP;
    public float m_MaxHP;
    public float m_MaxMP;
}

/// <summary>
/// NPC的数据
/// </summary>
public class NPC_Info : ObjectInfo
{
    public int m_plotId = 0; //0是不响应
    public NPC_Info(int plot, ObjectInfo info)
    {
        ID = info.ID;
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = MonsterType.NPC;
    }
}

/// <summary>
/// 怪物的数据
/// </summary>
public class Monster_Info : ObjectInfo
{
    public Monster_Info(MonsterType type, ObjectInfo info)
    {
        ID = info.ID;
        m_name = info.m_name;
        m_pos = info.m_pos;
        m_res = info.m_res;
        m_type = type;
    }

}
