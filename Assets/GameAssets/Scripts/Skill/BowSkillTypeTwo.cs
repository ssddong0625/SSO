using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSkillTypeTwo 
{
    public IEnumerator AttackSpeedChangeCo(Bow bow)
    {
        bow.FastAttack = 10;
        bow.Test = 10;
        yield return new WaitForSeconds(5f);
        bow.FastAttack = 1;
        bow.Test = 1;
        
    }
    
}
