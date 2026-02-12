using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class BossAoeExample : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    int atk = 5555;
    IEnumerator BossPatternCO()
    {
        float patternStart = 0.0f;
        float patternEnd = 5f;
        float startSize = 0.1f;
        float endSize = 50f;
        

        prefab.transform.localScale = new Vector3 (startSize, startSize, startSize);
        while (patternStart < patternEnd)
        {
            patternStart += Time.deltaTime;


            float t = patternStart / patternEnd;
            t = Mathf.Clamp(t, 0f, 1f);

            float scale = Mathf.Lerp(startSize, endSize, t);

            prefab.transform.localScale=new Vector3 (scale, scale, scale);

            yield return null;


        }

        prefab.transform.localScale= new Vector3(endSize, endSize, endSize);
    }

    public void Start()
    {
        Patter();
    }
    public void Patter()
    {
        StartCoroutine(BossPatternCO());
    }
    private void OnTriggerEnter(Collider other)
    {
        IHitAble hit = other.GetComponent<IHitAble>();

        hit.Hit(atk);
    }
}

