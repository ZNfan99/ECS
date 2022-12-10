using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class RoleEditor : EditorWindow
{
    public Player m_player;
    public PlayerInfo m_info;

    public List<string> m_skillnames = new List<string>();
    public List<string> m_names = new List<string>() { "1", "2", "3", "4" };
    int m_index = -1;
    string m_skillName = "";
    [MenuItem("Tools/SkillEditor")]
    public static void Init()
    {
        RoleEditor window = EditorWindow.GetWindow<RoleEditor>("角色编辑器");
        if (window != null)
        {
            string path = "Assets/PlayerConfig.txt";
            if (File.Exists(path))
            {
                string str = File.ReadAllText(path);
                window.m_info = JsonConvert.DeserializeObject<PlayerInfo>(str);
                if (window.m_info != null)
                {
                    window.m_index = window.m_info.index;
                    window.m_player = Player.Init(window.m_names[window.m_index]);
                    window.m_skillnames = window.m_player.lists;
                }
                else
                {
                    window.m_info = new PlayerInfo();
                }
            }
            else
            {
                window.m_info = new PlayerInfo();
            }
            window.Show();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择人物：", GUILayout.Width(70));
        int index = EditorGUILayout.Popup(m_index, m_names.ToArray());
        if (index != m_index)
        {
            if (m_player != null)
            {
                m_player.Destroy();
            }
            m_index = index;
            m_info.prefabName = m_names[m_index];
            m_info.index = index;
            m_player = Player.Init(m_names[m_index]);
            m_skillnames = m_player.lists;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("技能名字：", GUILayout.Width(70));
        m_skillName = EditorGUILayout.TextField(m_skillName, GUILayout.Width(150));
        GUILayout.Space(15);
        if (GUILayout.Button("添加技能", GUILayout.Width(70)))
        {
            if (m_player != null && m_skillName != "")
            {
                m_skillnames.Add(m_skillName);
                List<SkillBase> skills = m_player.AddNewSkill(m_skillName);
                OpenWindow(m_skillName, skills);
                m_skillName = "";
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.Label("技能列表", GUILayout.Width(100));

        GUILayout.BeginScrollView(Vector2.zero, false, true);

        foreach (var item in m_skillnames)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();   
            GUILayout.Space(40);
            if (GUILayout.Button(item, GUILayout.Width(150)))
            {
                List<SkillBase> skills = m_player.GetSkill(item);
                foreach (var ite in skills)
                {
                    ite.Init();
                }
                OpenWindow(item, skills);
            }
            GUILayout.Space(20);
            if (GUILayout.Button("删除", GUILayout.Width(70)))
            {
                m_player.RemoveSkill(item);
                m_skillnames.Remove(item);
                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("保存配置"))
        {
            string str = JsonConvert.SerializeObject(m_info);
            File.WriteAllText("Assets/PlayerConfig.txt", str);
        }
        GUILayout.EndVertical();
    }

    private void OpenWindow(string name, List<SkillBase> skills)
    {
        SkillEditor window = EditorWindow.GetWindow<SkillEditor>(name);
        if (window != null)
        {
            window.InitData(m_player, skills, name);
            window.Show();
        }
    }
}
