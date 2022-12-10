using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;

public class Player : MonoBehaviour
{
    public Dictionary<string, List<SkillBase>> m_skillBaseDic = new Dictionary<string, List<SkillBase>>();
    public Dictionary<string, Skill> m_skillDic = new Dictionary<string, Skill>();
    public List<string> lists = new List<string>();
    public AnimatorOverrideController m_overrideController;
    RuntimeAnimatorController m_runtimeController;
    Animator m_animator;
    AudioSource m_audio;
    public Transform m_effectParent;
    
    public void Play(string name)
    {
        if (m_skillBaseDic.ContainsKey(name))
        {
            foreach (var item in m_skillBaseDic[name])
            {
                StartCoroutine("PlayerPlay", item);
            }
        }
    }

    IEnumerator PlayerPlay(SkillBase skill)
    {
        yield return new WaitForSeconds(skill.delayTime);
        skill.Init();
        skill.Play();
        if(skill is SkillEffect)
        {
            StartCoroutine("SetEffect", skill as SkillEffect);
        }
    }

    IEnumerator SetEffect(SkillEffect skill)
    {
        yield return new WaitForSeconds(1);
        skill.m_obj.SetActive(false);
    }

    public void Stop(string name)
    {
        if (m_skillBaseDic.ContainsKey(name))
        {
            foreach (var item in m_skillBaseDic[name])
            {
                item.Stop();
            }
        }
    }

    public List<SkillBase> AddNewSkill(string skillname)
    {
        if (!m_skillBaseDic.ContainsKey(skillname))
        {
            m_skillBaseDic.Add(skillname, new List<SkillBase>());
            m_skillDic.Add(skillname, new Skill());

        }
        return m_skillBaseDic[skillname];
    }

    public List<SkillBase> GetSkill(string skillname)
    {
        if (m_skillBaseDic.ContainsKey(skillname))
        {
            return m_skillBaseDic[skillname];
        }
        return new List<SkillBase>();
    }

    public void RemoveSkill(string skillname)
    {
        if (m_skillBaseDic.ContainsKey(skillname))
        {
            m_skillBaseDic.Remove(skillname);
            m_skillDic.Remove(skillname);
        }
    }

    public void InitData()
    {
        m_overrideController = new AnimatorOverrideController();
        m_runtimeController = Resources.Load<RuntimeAnimatorController>("Player");
        m_animator = GetComponent<Animator>();
        m_overrideController.runtimeAnimatorController = m_runtimeController;
        m_animator.runtimeAnimatorController = m_overrideController;
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
        m_audio = GetComponent<AudioSource>();
        m_effectParent = transform.Find("EffectParent").transform;
        LoadJson();
    }

    public static Player Init(string name)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(name));
        if (obj != null)
        {
            obj.name = name;
            Player player = obj.GetComponent<Player>();
            player.m_overrideController = new AnimatorOverrideController();
            player.m_runtimeController = Resources.Load<RuntimeAnimatorController>("Player");
            player.m_animator = obj.GetComponent<Animator>();
            player.m_overrideController.runtimeAnimatorController = player.m_runtimeController;
            player.m_animator.runtimeAnimatorController = player.m_overrideController;
            if (obj.GetComponent<AudioSource>() == null)
            {
                obj.AddComponent<AudioSource>();
            }
            player.m_audio = obj.GetComponent<AudioSource>();
            player.m_effectParent = obj.transform.Find("EffectParent").transform;          
            player.LoadJson();
            return player;
        }
        return null;
    }

    private void LoadJson()
    {
        string path = "Assets/" + name + ".txt";
        if (File.Exists(path))
        {
            string str = File.ReadAllText(path);
            List<Skill> skills = JsonConvert.DeserializeObject<List<Skill>>(str);
            foreach (var item in skills)
            {
                m_skillBaseDic.Add(item.name, new List<SkillBase>());
                m_skillDic.Add(item.name, item);
                lists.Add(item.name);
                foreach (var ite in item.dic)
                {
                    for (int i = 0; i < ite.Value.Count; i++)
                    {
                        if (ite.Key.Equals("动画"))
                        {
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(ite.Value[i].path);
                            SkillAnim anim = new SkillAnim(this);
                            anim.SetAnimClip(clip, ite.Value[i].path);
                            anim.SetDelayTime(ite.Value[i].delaytime);
                            m_skillBaseDic[item.name].Add(anim);
                        }
                        else if (ite.Key.Equals("音效"))
                        {
                            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(ite.Value[i].path);
                            SkillAudio audio = new SkillAudio(this);
                            audio.SetAudioClip(clip, ite.Value[i].path);
                            audio.SetDelayTime(ite.Value[i].delaytime);
                            m_skillBaseDic[item.name].Add(audio);
                        }
                        else if (ite.Key.Equals("特效"))
                        {
                            GameObject clip = AssetDatabase.LoadAssetAtPath<GameObject>(ite.Value[i].path);
                            SkillEffect effect = new SkillEffect(this);
                            effect.SetEffectClip(clip, ite.Value[i].path);
                            effect.SetDelayTime(ite.Value[i].delaytime);
                            m_skillBaseDic[item.name].Add(effect);
                        }
                    }
                }
            }
        }
    }
    public void SaveJson()
    {
        List<Skill> skills = new List<Skill>();
        foreach (var item in m_skillBaseDic)
        {
            Skill skill = new Skill();
            skill.name = item.Key;
            skill.angle = m_skillDic[item.Key].angle;
            skill.scope = m_skillDic[item.Key].scope;
            skill.hurt = m_skillDic[item.Key].hurt;
            foreach (var ite in item.Value)
            {
                if (ite is SkillAnim)
                {
                    if (!skill.dic.ContainsKey("动画"))
                    {
                        skill.dic.Add("动画", new List<SkillClip>());
                    }
                    SkillClip clip = new SkillClip();
                    clip.delaytime = ite.delayTime;
                    clip.path = ite.path;
                    skill.dic["动画"].Add(clip);
                }
                else if (ite is SkillAudio)
                {
                    if (!skill.dic.ContainsKey("音效"))
                    {
                        skill.dic.Add("音效", new List<SkillClip>());
                    }
                    SkillClip clip = new SkillClip();
                    clip.delaytime = ite.delayTime;
                    clip.path = ite.path;
                    skill.dic["音效"].Add(clip);
                }
                else if (ite is SkillEffect)
                {
                    if (!skill.dic.ContainsKey("特效"))
                    {
                        skill.dic.Add("特效", new List<SkillClip>());
                    }
                    SkillClip clip = new SkillClip();
                    clip.delaytime = ite.delayTime;
                    clip.path = ite.path;
                    skill.dic["特效"].Add(clip);
                }
            }
            skills.Add(skill);
        }
        string str = JsonConvert.SerializeObject(skills);
        File.WriteAllText("Assets/" + name + ".txt", str);
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
