using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ÑªÌõ Ãû×Ö buff
/// </summary>
public class UIPate : MonoBehaviour
{
    private GameObject m_go;

    public Text m_name;
    public Scrollbar m_hp;
    public Scrollbar m_mp;
    public GameObject m_gather;
    public List<Image> m_gathers;
    public void InitPate()
    {
        m_go = GameObject.Instantiate(Resources.Load<GameObject>("Pate"), UIMgr.GetT().m_hudroot.transform);
        m_go.transform.localPosition = Vector3.zero;
        m_go.transform.localScale = Vector3.one;

        m_name = m_go.transform.Find("Name").GetComponent<Text>();
        m_hp = m_go.transform.Find("Hp").GetComponent<Scrollbar>();
        m_mp = m_go.transform.Find("Mp").GetComponent<Scrollbar>();
        m_gather = m_go.transform.Find("Gather").gameObject;
        m_gathers = new List<Image>();
        for (int i = 0; i < m_gather.transform.childCount; i++)
        {
            m_gathers.Add(m_gather.transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void Show(bool active)
    {
        if (m_go)
        {
            m_go.SetActive(active);
        }
    }
    public void SetData(int gather)
    {
        for (int i = 0; i < m_gathers.Count; i++)
        {
            m_gathers[i].gameObject.SetActive(i < gather);
        }
    }

    public void SetData(string  name)
    {
        m_name.text = name;
    }
    public void SetData(string name, float hp, float mp)
    {
        m_name.text = name;
        m_hp.size = hp;
        m_mp.size = mp;
    }


    Vector3 camerapos = Vector3.zero;
    private void Update()
    {
        camerapos.Set(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
        m_go.transform.position = World.GetT().m_main.WorldToScreenPoint(camerapos);
    }

    ~UIPate()
    {
        m_name = null;
        m_hp = null;
        m_mp = null;
    }
}
