using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

public class JsonData
{
    public List<MonsterData> datas = new List<MonsterData>();
}

[Serializable]
public class MonsterData
{
    public string name;
    public float x;
    public float y;
    public float z;
    public MonsterType type;
    public MonsterData(string name, float x, float y, float z, MonsterType type)
    {
        this.name = name;
        this.x = x;
        this.y = y;
        this.z = z;
        this.type = type;
    }
}

public class MonsterCfg : Singleton<MonsterCfg>
{
    public JsonData data;
    string path;
    public void Init()
    {
        path = "Assets/monster.txt";
        string json = File.ReadAllText(path);
        data = JsonConvert.DeserializeObject<JsonData>(json);
    }

    public JsonData GetJsonDate()
    {
        return data;
    }
}
