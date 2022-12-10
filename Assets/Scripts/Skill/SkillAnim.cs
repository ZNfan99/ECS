using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnim : SkillBase
{
    public Player m_player;
    public AnimatorOverrideController m_overrideController;
    public AnimationClip m_clip;
    public Animator m_animator;

    public SkillAnim(Player _player)
    {
        m_player = _player;
        m_overrideController = _player.m_overrideController;
        m_animator = _player.gameObject.GetComponent<Animator>();
    }

    public void SetAnimClip(AnimationClip _clip,string _path)
    {
        m_clip = _clip;
        path = _path;
        m_overrideController["Start"] = m_clip;
    }
    public override void Init()
    {
        base.Init();
        m_overrideController["Start"] = m_clip;
    }

    public override void Play()
    {
        base.Play();
        m_animator.StopPlayback();
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Idle") || stateInfo.IsName("Run"))
        {
            m_animator.SetTrigger("Play");
        }
    }

    public override void SetDelayTime(float time)
    {
        base.SetDelayTime(time);
    }

    public override void Stop()
    {
        base.Stop();
        m_animator.StopPlayback();
    }
}
