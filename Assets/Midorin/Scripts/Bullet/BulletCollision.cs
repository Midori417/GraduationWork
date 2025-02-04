using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾当たり衝突判定コンポーネント
/// </summary>
public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("与えるダメージ")]
    private int _atk = 0;

    [SerializeField, Header("ダウン値")]
    private int _down = 0;

    [SerializeField, Header("衝突したときに自身を破壊するか")]
    private bool _isHitDestroy = false;

    [SerializeField, Header("衝突時のエフェクト")]
    private GameObject _pfbEffHit = null;

    [SerializeField, Header("生成してからエフェクト消すまで")]
    private float _hitEffDestroyTimer = 0;

    private GameTimer _collisionTimer = new GameTimer(0.1f);

    // trueなら衝突可能
    private bool _isCollision = false;

    #region イベント関数

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (_collisionTimer.UpdateTimer())
        {
            _isCollision = true;
        }
    }

    /// <summary>
    /// 衝突したときに処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_isCollision) return;

        // エフェクトを生成
        if (other.CompareTag("MsCollision") || (other.CompareTag("Ground") || other.CompareTag("Building")))
            CreteHitEffect();

        // 機体に衝突したらダメージを与える
        if (other.CompareTag("MsCollision"))
        {
            other.GetComponent<MsDamage>().Damage(_atk, _down, transform.position);
            if (_isHitDestroy) Destroy(gameObject);
        }

        // 建物に衝突したら自身を破壊
        if (other.CompareTag("Ground") || other.CompareTag("Building"))
        {
            Destroy(gameObject);
        }
    }

    #endregion

    /// <summary>
    /// ヒットエフェクト生成
    /// </summary>
    private void CreteHitEffect()
    {
        // エフェクトを生成
        if (_pfbEffHit)
        {
            var obj = Instantiate(_pfbEffHit, transform.position, transform.rotation);
            Destroy(obj, _hitEffDestroyTimer);
        }
    }
}
