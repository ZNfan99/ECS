using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInit : MonoBehaviour
{
    public GameObject[] DontDestory;
    public List<Button> btnskills;
    public ETCJoystick Joystick;
    public GameObject uiroot;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < DontDestory.Length; i++)
        {
            DontDestroyOnLoad(DontDestory[i]);
        }
        GameSceneUtils.GetT().LoadSceneAsync("BattleScene", () => {
            JoyStickMgr.GetT().m_joyGO = DontDestory[0];
            JoyStickMgr.GetT().m_joystick = Joystick;
            JoyStickMgr.GetT().m_skillBtn = btnskills;

            //配置数据解析
            GameData.GetT().InitByRoleName("1");
            MonsterCfg.GetT().Init();
            //任务配置表解析
            GameData.GetT().InitTaskData();

            World.GetT().Init();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
