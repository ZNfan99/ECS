using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase
{
    public string name;
    public string path;
    public float delayTime;

    public virtual void Init()
    {

    }

    public virtual void Play()
    {

    }

    public virtual void Stop()
    {

    }

    public virtual void SetDelayTime(float time)
    {
        delayTime = time;
    }
}
