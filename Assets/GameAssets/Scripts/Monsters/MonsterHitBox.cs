using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour,IHitAble
{
    [SerializeField]
    Monster monster;
    // Start is called before the first frame update


    public void Hit(int atk)
    {
        monster.Hp -= atk;
        //monster.text.text = $"{atk}";
        UiManager.instance.MonsterHpView(monster);

      //  StartCoroutine(DamageCo());
    }

    IEnumerator DamageCo()
    {
        
        yield return new WaitForSeconds(0.5f);
        monster.text.text = null;
    }
}
