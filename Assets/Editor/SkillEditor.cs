using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SkillEditor : EditorWindow
{
    Player m_player;
    List<SkillBase> m_skills;
    string m_name;
    int m_index;
    List<string> types = new List<string>() { "动画", "声音", "特效" };
    public void InitData(Player _player, List<SkillBase> skills, string name)
    {
        m_player = _player;
        m_skills = skills;
        m_name = name;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("范围：", GUILayout.Width(50));
        m_player.m_skillDic[m_name].scope = EditorGUILayout.FloatField(m_player.m_skillDic[m_name].scope, GUILayout.Width(50));
        GUILayout.Label("角度：", GUILayout.Width(50));
        m_player.m_skillDic[m_name].angle = EditorGUILayout.IntField(m_player.m_skillDic[m_name].angle, GUILayout.Width(50));
        GUILayout.Label("伤害：", GUILayout.Width(50));
        m_player.m_skillDic[m_name].hurt = EditorGUILayout.IntField(m_player.m_skillDic[m_name].hurt, GUILayout.Width(50));
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("播放"))
        {
            m_player.Play(m_name);
        }
        if (GUILayout.Button("暂停"))
        {
            m_player.Stop(m_name);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("技能类型：", GUILayout.Width(70));
        m_index = EditorGUILayout.Popup(m_index, types.ToArray());
        if (GUILayout.Button("添加", GUILayout.Width(70)))
        {
            if (m_index == 0)
            {
                m_skills.Add(new SkillAnim(m_player));
            }
            else if (m_index == 1)
            {
                m_skills.Add(new SkillAudio(m_player));
            }
            else if (m_index == 2)
            {
                m_skills.Add(new SkillEffect(m_player));
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUILayout.BeginScrollView(Vector2.zero, false, true);
        foreach (var item in m_skills)
        {
            if (GUILayout.Button("删除"))
            {
                m_skills.Remove(item);
                break;
            }
            if (item is SkillAnim)
            {
                ShowSkillAnim(item as SkillAnim);
            }
            else if(item is SkillAudio)
            {
                ShowSkillAudio(item as SkillAudio);
            }
            else if (item is SkillEffect)
            {
                ShowSkillEffect(item as SkillEffect);
            }
        }
        GUILayout.EndScrollView();

        if (GUILayout.Button("保存技能"))
        {
            m_player.SaveJson();
        }
        GUILayout.EndVertical();
    }

    private void ShowSkillEffect(SkillEffect skillEffect)
    {
        GameObject clip = EditorGUILayout.ObjectField(skillEffect.m_clip, typeof(GameObject), false) as GameObject;
        if (clip != skillEffect.m_clip)
        {
            skillEffect.m_clip = clip;
            string path = AssetDatabase.GetAssetPath(clip);
            skillEffect.SetEffectClip(clip, path);
        }
        SetTime(skillEffect);
    }
    private void ShowSkillAudio(SkillAudio skillAudio)
    {
        AudioClip clip = EditorGUILayout.ObjectField(skillAudio.m_clip, typeof(AudioClip), false) as AudioClip;
        if (clip != skillAudio.m_clip)
        {
            skillAudio.m_clip = clip;
            string path = AssetDatabase.GetAssetPath(clip);
            skillAudio.SetAudioClip(clip, path);
        }
        SetTime(skillAudio);
    }

    private void ShowSkillAnim(SkillAnim skillAnim)
    {
        AnimationClip clip = EditorGUILayout.ObjectField(skillAnim.m_clip, typeof(AnimationClip), false) as AnimationClip;
        if (clip != skillAnim.m_clip)
        {
            skillAnim.m_clip = clip;
            string path = AssetDatabase.GetAssetPath(clip);
            skillAnim.SetAnimClip(clip, path);
        }
        SetTime(skillAnim);
    }
    public void SetTime(SkillBase skill)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("延迟时间：", GUILayout.Width(70));
        float t = EditorGUILayout.FloatField(skill.delayTime, GUILayout.Width(200));
        if (t != skill.delayTime)
        {
            skill.delayTime = t;
            skill.SetDelayTime(t);
        }
        GUILayout.EndHorizontal();
    }
}
