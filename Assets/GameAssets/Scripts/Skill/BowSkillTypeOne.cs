using GameAssets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSkillTypeOne
{
    //Coroutine coroutine;
   public IEnumerator ChangeCo(Bow bow)
    {
        Debug.Log("¹ßµ¿µÊ");
        bow.CurrentKey = bow.ChangeKey;
        bow.EnsurePool(bow.CurrentKey);
        yield return new WaitForSeconds(3f);
        bow.CurrentKey = bow.Key;
        bow.EnsurePool(bow.CurrentKey);
    }
   

}
