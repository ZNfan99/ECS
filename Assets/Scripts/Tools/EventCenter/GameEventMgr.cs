using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ��Ϣ����
/// </summary>
public class MsgCenter : Singleton<MsgCenter>
{
    Dictionary<string, Action<Notification>> m_MsgDicts = new Dictionary<string, Action<Notification>>();
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddListener(string name, Action<Notification> action)
    {
        if (!m_MsgDicts.ContainsKey(name))
        {
            m_MsgDicts.Add(name, null);
        }
        m_MsgDicts[name] += action;
    }

    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="action"></param>
    public void RemoveListener(string msg, Action<Notification> action)
    {
        if (m_MsgDicts.ContainsKey(msg))
        {
            m_MsgDicts[msg] -= action;
            if (m_MsgDicts[msg] == null)
            {
                m_MsgDicts.Remove(msg);
            }
        }
    }

    /// <summary>
    /// �㲥
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="notify"></param>
    public void SendMsg(string msg, Notification notify)
    {
        if (m_MsgDicts.ContainsKey(msg))
        {
            m_MsgDicts[msg].Invoke(notify);
        }
    }
}

/// <summary>
/// ��Ϣ����ö��
/// </summary>
/// <summary>
/// ��Ϣ��
/// </summary>
public class Notification
{
    //��Ϣ����
    public string msg;
    //�洢��Ϣ�ļ���
    public object[] data;

    public void Refresh(string msg, params object[] data)
    {
        this.msg = msg;
        this.data = data;
    }
    public void Clear()
    {
        msg = string.Empty;
        data = null;
    }
}
