using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 消息中心
/// </summary>
public class MsgCenter : Singleton<MsgCenter>
{
    Dictionary<string, Action<Notification>> m_MsgDicts = new Dictionary<string, Action<Notification>>();
    /// <summary>
    /// 添加侦听
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
    /// 删除侦听
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
    /// 广播
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
/// 消息类型枚举
/// </summary>
/// <summary>
/// 消息类
/// </summary>
public class Notification
{
    //消息类型
    public string msg;
    //存储消息的集合
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
