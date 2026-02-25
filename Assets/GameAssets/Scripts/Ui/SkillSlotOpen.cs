using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlotOpen : MonoBehaviour
{
    [Header("활 전용 스킬창")]
    [SerializeField]
    GameObject bowOpen;
    [SerializeField]
    GameObject levelFiveOpen;
    [SerializeField]
    GameObject levelTenOpen;
    [SerializeField]
    GameObject levelFifthOpen;
    [SerializeField]
    GameObject levelTwentyOpen;

    public GameObject LevelFiveOpen => levelFiveOpen;
    public GameObject LevelTenOpen => levelTenOpen;
    public GameObject LevelFifthOpen => levelFifthOpen;
    public GameObject LevelTwentyOpen => levelTwentyOpen;



   
}
