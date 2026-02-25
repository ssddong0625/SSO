using GameAssets.Scripts.Manager;
using GameAssets.Scripts.Monsters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSkillTypeThree
{
    [SerializeField] private int multiShotCount = 5;
    [SerializeField] private float multiShotSpread = 20f;
    [SerializeField] private float multiShotRange = 100f;

    public void SkillThree(Bow bow)
    {
        Vector3 firePoint =
            GameManager.instance.Player.transform.position
            + GameManager.instance.Player.transform.forward * 0.6f
            + Vector3.up * 1.2f;

        Ray centerRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        for (int i = 0; i < multiShotCount; i++)
        {
            float angle = GetSpreadAngle(i, multiShotCount, multiShotSpread); // [추가] i번째 발의 퍼짐 각도

            // [추가] 좌우로 퍼지도록 Y축 기준 회전
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * centerRay.direction;
            Ray shotRay = new Ray(centerRay.origin, dir);

            Vector3 hitPoint = shotRay.origin + shotRay.direction * multiShotRange;

            RaycastHit hit;
            Monster mon = null;
            IHitAble hits = null;

            if (Physics.Raycast(shotRay, out hit, 100f, bow.LayerMask))
            {
                hitPoint = hit.point;
                mon = hit.collider.GetComponentInParent<Monster>();
                hits = hit.collider.GetComponent<IHitAble>();
            }

            if (hits != null)
            {
                mon?.SetAttacker(bow.transform);
                hits.Hit(bow.data.atk); //필요하면: (int)(data.atk * 0.7f) 로 멀티샷 개별 데미지 조절 가능
            }

            //추가] 발마다 화살 연출도 쏘기
            if (firePoint != null)
                bow.SpawnArrowVfx(firePoint, hitPoint);
        }
    }

    //[추가] 퍼짐 각도 계산 (부채꼴 균등 분배)
    private float GetSpreadAngle(int index, int count, float totalSpread)
    {
        if (count <= 1) return 0f;
        float t = (float)index / (count - 1); // 0..1
        return Mathf.Lerp(-totalSpread * 0.5f, totalSpread * 0.5f, t);
    }





}
