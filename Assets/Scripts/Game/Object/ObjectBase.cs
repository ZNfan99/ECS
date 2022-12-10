using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物的类型（不一定是怪物，区别于玩家）
/// </summary>
public enum MonsterType
{
    Null = 0,
    Normal, //怪物
    Gather, //采集物
    Biaoche, //跟随物
    NPC,//
}


public abstract class ObjectBase
{
    public GameObject m_go;//这是游戏对象
    public Vector3 m_currentPos;//游戏对象在场景中的位置
    public Animator m_anim;
    public UIPate m_pate;
    public MonsterType m_type;

    public int m_insID;//唯一ID
    public string m_modelPath;//模型路径

    public ObjectBase()
    {

    }

    /// <summary>
    /// 创建物体实体的方法
    /// </summary>
    /// <param name="type"></param>
    public virtual void CreateObj(MonsterType type)
    {
        m_type = type;
        if(!string.IsNullOrEmpty(m_modelPath) && m_insID >=0)
        {
            m_go = GameObject.Instantiate(Resources.Load<GameObject>(m_modelPath));
            m_go.name = m_modelPath;
            if (m_type == MonsterType.Null)
            {
                m_go.tag = "Player";
                m_go.name = 1.ToString();
            }
            m_go.transform.position = m_currentPos;
            if(m_go)
            {
                OnCreate();
            }
        }
    }

    /// <summary>
    /// 创建的时候进行初始化的逻辑
    /// </summary>
    public virtual void OnCreate()
    {
        
    }
    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="pos"></param>
    public virtual void SetPos(Vector3 pos)
    {
        m_currentPos = pos;
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="lookforward"></param>
    /// <param name="move"></param>
    public void Move(Vector3 lookforward,Vector3 move)
    {
        m_go.transform.LookAt(lookforward);
        m_go.transform.Translate(move);
    }
    public void MoveByTranslate(Vector3 look, Vector3 move)
    {

        m_go.transform.LookAt(look);
        m_go.transform.Translate(move);
    }
    public void AutoMove(Vector3 look, Vector3 move)
    {

        MoveByTranslate(look, move);
    }
    /// <summary>
    /// 销毁物体的方法
    /// </summary>
    public virtual void Destroy()
    {
        if (m_pate)
        {
            GameObject.Destroy(m_pate);
        }
        GameObject.Destroy(m_go.gameObject);
        m_currentPos = Vector3.zero;
        m_anim = null;
        m_insID = -1;
    }
}
