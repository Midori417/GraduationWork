using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("生成から衝突可能までの時間")]
    private float hitTimer = 0.1f;

    private bool isHit = false;

    [SerializeField, Header("与えるダメージ")]
    private int damage;

    [SerializeField, Header("ダウン値")]
    private int downValue;

    [SerializeField, Header("衝突したときに自身を破壊するか")]
    private bool isDead = false;

    [SerializeField, Header("衝突時のエフェクト")]
    private GameObject pfb_eff_hit;

    [SerializeField, Header("生成してからエフェクト消すまで")]
    private float hit_eff_destroyTimer;

    private void Start()
    {
        Invoke("HitStart", hitTimer);
    }

    void HitStart()
    {
        isHit = true;
    }

    /// <summary>
    /// ヒットエフェクト生成
    /// </summary>
    private void CreteHitEffect()
    {
        // エフェクトを生成
        if (pfb_eff_hit)
        {
            var obj =Instantiate(pfb_eff_hit, transform.position, transform.rotation);
            Destroy(obj, hit_eff_destroyTimer);
        }
    }

    /// <summary>
    /// 衝突したときに処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(!isHit)
        {
            return;
        }


        // 機体に衝突したらダメージを与える
        if (other.gameObject.tag == "MsCollision")
        {
            // エフェクトを生成
            CreteHitEffect();
            other.GetComponent<MsDamageCheck>().Damage(damage, downValue, transform.position);
            if(isDead)
            {
                Destroy(gameObject);
            }
        }

        // 建物に衝突したら自身を破壊
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Building")
        {
            // エフェクトを生成
            CreteHitEffect();

            Destroy(gameObject);
        }
    }
}
