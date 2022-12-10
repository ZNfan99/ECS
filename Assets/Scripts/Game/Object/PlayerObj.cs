using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有的游戏玩家
/// </summary>
public class PlayerObj : ObjectBase
{
    public Player_Info m_info;

    public PlayerObj(Player_Info info)
    {
        m_info = info;
    }
    public override void Destroy()
    {
        base.Destroy();
    }

    public override void OnCreate()
    {
        base.OnCreate();
        m_pate = m_go.AddComponent<UIPate>();
        m_pate.InitPate();
        m_pate.SetData(m_info.m_name, m_info.m_HP / m_info.m_MaxHP, m_info.m_MP / m_info.m_MaxMP);
    }

    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }
}

/// <summary>
/// 自己创建控制的玩家
/// </summary>
public class HostPlayer : PlayerObj
{
    Player player;
    public HostPlayer(Player_Info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }

    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
        m_anim = m_go.GetComponent<Animator>();
    }

    public override void Destroy()
    {
        base.Destroy();
    }

    public override void OnCreate()
    {
        base.OnCreate();
        player = m_go.GetComponent<Player>();
        player.InitData();
        MsgCenter.GetT().AddListener("atkActionPlay", (notify) => {
            if (notify.msg.Equals("ByServer"))
            {
                int skillid = (int)notify.data[0];
            }

        });
    }

    public override void SetPos(Vector3 pos)
    {
        base.SetPos(pos);
    }
    Notification notify = new Notification();
    public void JoystickHandlerMoving(float h, float v)
    {
        if (Mathf.Abs(h) > 0.05f || (Mathf.Abs(v) > 0.05f))
        {
            MoveByTranslate(new Vector3(m_go.transform.position.x + h, m_go.transform.position.y, m_go.transform.position.z + v), Vector3.forward * Time.deltaTime * 1);
            notify.Refresh("Player", m_go.transform.position);
            MsgCenter.GetT().SendMsg("MovePos", notify);
            //m_anim.SetBool("Run", true);
        }
    }

    
    public void JoyButtonHandler(string btnName)
    {
        
        switch (btnName)
        {
            case "BtnSkill1":
                player.Play("1");

                Notification m_notify = new Notification();
                m_notify.Refresh("atkOther", 1, 2, 1);
                MsgCenter.GetT().SendMsg("ByClent_Battle", m_notify);
                break;
            case "BtnSkill2":
                player.Play("2");

                Notification m_notify2 = new Notification();
                m_notify2.Refresh("atkOther", 1, 2, 1);
                MsgCenter.GetT().SendMsg("ByClent_Battle", m_notify2);
                break;
            case "BtnSkill3":
                player.Play("3");

                Notification m_notify3 = new Notification();
                m_notify3.Refresh("atkOther", 1, 2, 1);
                MsgCenter.GetT().SendMsg("ByClent_Battle", m_notify3);
                break;
        }
    }
}

public class OtherPlayer : PlayerObj
{
    public OtherPlayer(Player_Info info) : base(info)
    {
        m_insID = info.ID;
        m_modelPath = info.m_res;
    }
    public override void CreateObj(MonsterType type)
    {
        SetPos(m_info.m_pos);
        base.CreateObj(type);
    }
}

