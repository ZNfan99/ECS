using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAudio : SkillBase
{
    Player m_player;
    AudioSource m_audio;
    public AudioClip m_clip;
    public void SetAudioClip(AudioClip _clip,string _path)
    {
        m_clip = _clip;
        path = _path;
        m_audio.clip = _clip;
    }
    public SkillAudio(Player _player)
    {
        m_player = _player;
        m_audio = m_player.gameObject.GetComponent<AudioSource>();
    }
    public override void Init()
    {
        base.Init();
        m_audio.clip = m_clip;
    }

    public override void Play()
    {
        base.Play();
        m_audio.Play();
    }

    public override void SetDelayTime(float time)
    {
        base.SetDelayTime(time);
    }

    public override void Stop()
    {
        base.Stop();
        m_audio.Stop();
    }
}
