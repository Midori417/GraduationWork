using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("与えるダメージ")]
    private int damage;

    [SerializeField, Header("ダウン値")]
    private float downValue;

    [SerializeField, Header("衝突したときに自身を破壊するか")]
    private bool isDead = false;

    [SerializeField, Header("衝突時のエフェクト")]
    private GameObject pfb_eff_hit;

    /// <summary>
    /// 衝突したときに処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // エフェクトを生成
        if(pfb_eff_hit)
        {
            Instantiate(pfb_eff_hit, transform.position, transform.rotation);
        }

        // 機体に衝突したらダメージを与える
        if (other.gameObject.tag == "MS")
        {
            other.GetComponent<BaseMs>().Damage(damage, transform.position);
            if(isDead)
            {
                Destroy(gameObject);
            }
        }

        // 建物に衝突したら自身を破壊
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Building")
        {
            Destroy(gameObject);
        }
    }
}
