using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : Singleton<World>
{
    public Dictionary<int, ObjectBase> m_insDic = new Dictionary<int, ObjectBase>();
    public HostPlayer m_plyer;
    private GameObject npcroot;
    public Camera m_main;

    public float xlength;
    public float ylength;

    public void Init()
    {
        GameObject plan = GameObject.Find("Plane");
        Vector3 length = plan.GetComponent<MeshFilter>().mesh.bounds.size;
        xlength = length.x * plan.transform.lossyScale.x;
        ylength = length.z * plan.transform.lossyScale.z;
        Debug.Log($"地图的尺寸为  x:{xlength}  y:{ylength}");
        npcroot = GameObject.Find("NPC_Root");
        m_main = GameObject.Find("Main Camera").GetComponent<Camera>();
        npcroot = GameObject.Find("NPC_Root");
        UIMgr.GetT().Init(GameObject.Find("UIRoot"), GameObject.Find("HUD"));

        Player_Info info = new Player_Info();
        info.ID = 0;
        info.m_name = "1";
        info.m_level = 9;
        info.m_pos = Vector3.zero;
        info.m_res = "1";
        info.m_HP = 2000;
        info.m_MP = 1000;
        info.m_MaxHP = 2000;
        info.m_MaxMP = 2000;

        m_plyer = new HostPlayer(info);
        m_plyer.CreateObj(MonsterType.Null);
        JoyStickMgr.GetT().SetJoyArg(m_main, m_plyer);
        JoyStickMgr.GetT().JoyActive = true;

        CreateIns();

        MsgCenter.GetT().AddListener("AutoMove", (notify) =>
        {
            this.AutoMoveByInsId((int)notify.data[0], (Vector3)notify.data[1]);
        });
    }
    private void CreateIns()
    {
        JsonData data = MonsterCfg.GetT().GetJsonDate();
        ObjectInfo info;

        for (int i = 0; i < data.datas.Count; i++)
        {
            info = new ObjectInfo();
            info.ID = m_insDic.Count + 1;
            info.m_name = data.datas[i].name;
            //info.m_name = string.Format("{0}({1})", data.datas[i].name, info.ID);
            info.m_res = data.datas[i].name;
            info.m_pos = new Vector3(data.datas[i].x, data.datas[i].y, data.datas[i].z);
            info.m_type = data.datas[i].type;
            CreateObj(info);
        }
    }
    ObjectBase monster = null;
    private void CreateObj(ObjectInfo info)
    {
        monster = null;
        if (info != null)
        {
            if (info.m_type == MonsterType.Normal)
            {
                monster = new Normal(info);
            }
            else if (info.m_type == MonsterType.Gather)
            {
                monster = new Gather(info);
            }
            else if (info.m_type == MonsterType.NPC)
            {
                monster = new NpcObj(1, info);
            }
        }
        if (monster != null)
        {
            monster.CreateObj(info.m_type);
            monster.m_go.transform.SetParent(npcroot.transform, false);
            m_insDic.Add(info.ID, monster);
        }
        else
        {
            Debug.Log("生成失败!!!!");
        }

    }
    public void AutoMoveByInsId(int target,Vector3 pos)
    {
        using (var tmp = m_insDic.GetEnumerator())
        {
            while (tmp.MoveNext())
            {
                if (target == tmp.Current.Key)
                {               
                    //TODO  让实例移动
                    tmp.Current.Value.AutoMove(pos, pos);
                }
            }
        }
    }
}
