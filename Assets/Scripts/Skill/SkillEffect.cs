using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect : SkillBase
{
    Player m_player;
    public GameObject m_obj;
    ParticleSystem m_ps;
    public GameObject m_clip;

    public SkillEffect(Player _player)
    {
        m_player = _player;
    }

    public void SetEffectClip(GameObject _clip,string _path)
    {
        m_clip = _clip;
        path = _path;
        if(m_clip.GetComponent<ParticleSystem>())
        {
            if(m_obj != null)
            {
                GameObject.Destroy(m_obj);
            }
            m_obj = GameObject.Instantiate(m_clip, m_player.m_effectParent);
            m_obj.SetActive(false);
            m_ps = m_obj.GetComponent<ParticleSystem>();
        }
    }

    public override void Init()
    {
        base.Init();
        m_ps = m_obj.GetComponent<ParticleSystem>();
    }

    public override void Play()
    {
        base.Play();
        m_obj.SetActive(true);
 
    }

    public override void SetDelayTime(float time)
    {
        base.SetDelayTime(time);
    }

    public override void Stop()
    {
        base.Stop();
        m_ps.Stop();
    }
}
