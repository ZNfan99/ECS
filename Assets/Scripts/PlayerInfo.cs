using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerInfo
{
    public string prefabName;
    public int index;
}

public class Skill
{
    public string name;
    public int angle;
    public float scope;
    public int hurt;
    public Dictionary<string, List<SkillClip>> dic = new Dictionary<string, List<SkillClip>>();
}

public class SkillClip
{
    public float delaytime;
    public string path;
}
