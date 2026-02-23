using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill
{
    [SerializeField]
    float skillGauge = 0f;
    [SerializeField]
    float skillMaxGauge = 100f;
    bool skillModeTest = false;
    bool next = false;
    

    public event Action onSkill;
    public float SkillGague => skillGauge;
    public float SkillMaxGague => skillMaxGauge;
    // Start is called before the first frame update
  
    public void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            skillModeTest = !next;
            next = skillModeTest;
            Debug.Log(skillModeTest);

        }
        if (skillModeTest)
        {

            if (Input.GetMouseButton(0))
            {
                Debug.Log($"스킬 두번쨰 진입,{skillGauge}");
                skillGauge++;
                if (skillGauge >= 100f)
                {
                    skillGauge = 100f;
                    
                }
                onSkill?.Invoke();
            }
            else
            {
                if (skillGauge == 100)
                {
                    
                }
                Debug.Log($"스킬 세번쨰 진입,{skillGauge}");
                skillGauge--;
                if (skillGauge <= 0)
                {
                    skillGauge = 0;
                }
                onSkill?.Invoke();
            }
        }
        else
        {
            skillGauge = 0;
        }


    }


}
