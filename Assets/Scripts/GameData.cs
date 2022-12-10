using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// 数据
/// </summary>
public class GameData : Singleton<GameData>
{
    public Dictionary<string, List<Skill>> roleSkillDic = new Dictionary<string, List<Skill>>();

    public void InitByRoleName(string roleName)
    {
        if(File.Exists("Assets/"+roleName+".txt"))
        {
            string str = File.ReadAllText("Assets/" + roleName + ".txt");
            List<Skill> skills = JsonConvert.DeserializeObject<List<Skill>>(str);
            roleSkillDic.Add(roleName,skills);
        }
    }

    public List<Skill> GetSkillByRoleName(string roleName)
    {
        if(roleSkillDic.ContainsKey(roleName))
        {
            return roleSkillDic[roleName];
        }
        return null;
    }
    public Dictionary<int, TaskData> AllTaskDic = new Dictionary<int, TaskData>();
    public void InitTaskData()
    {
        TaskData task = new TaskData();
        task.taskId = 1;
        task.taskName = "任务1";
        AllTaskDic.Add(task.taskId, task);
    }
    public TaskData GetTaskDataById(int taskId)
    {
        if (AllTaskDic.ContainsKey(taskId))
        {
            return AllTaskDic[taskId];
        }
        return null;
    }
}

public class TaskData
{
    public int taskId;
    public string taskName;
}
