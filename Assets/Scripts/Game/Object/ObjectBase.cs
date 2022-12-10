using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ͣ���һ���ǹ����������ң�
/// </summary>
public enum MonsterType
{
    Null = 0,
    Normal, //����
    Gather, //�ɼ���
    Biaoche, //������
    NPC,//
}


public abstract class ObjectBase
{
    public GameObject m_go;//������Ϸ����
    public Vector3 m_currentPos;//��Ϸ�����ڳ����е�λ��
    public Animator m_anim;
    public UIPate m_pate;
    public MonsterType m_type;

    public int m_insID;//ΨһID
    public string m_modelPath;//ģ��·��

    public ObjectBase()
    {

    }

    /// <summary>
    /// ��������ʵ��ķ���
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
    /// ������ʱ����г�ʼ�����߼�
    /// </summary>
    public virtual void OnCreate()
    {
        
    }
    /// <summary>
    /// ����λ��
    /// </summary>
    /// <param name="pos"></param>
    public virtual void SetPos(Vector3 pos)
    {
        m_currentPos = pos;
    }
    /// <summary>
    /// �ƶ�
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
    /// ��������ķ���
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
