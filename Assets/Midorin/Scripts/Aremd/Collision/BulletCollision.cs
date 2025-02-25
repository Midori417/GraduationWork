using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり衝突判定コンポーネント
/// </summary>
public class BulletCollision : BaseAttackCollision
{
    [SerializeField, Header("衝突したときに自身を破壊するか")]
    private bool _isHitDestroy = false;

    [SerializeField, Header("衝突時のエフェクト")]
    private GameObject _pfbEffHit = null;

    [SerializeField, Header("生成してからエフェクト消すまで")]
    private float _hitEffDestroyTimer = 0;

    private GameTimer _collisionTimer = new GameTimer(0.1f);

    [SerializeField, Header("着弾したときになる音")]
    private AudioClip _seHit;

    // trueなら衝突可能
    private Collider _collider;

    #region イベント関数

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        // 一定時間たったら衝突判定を起動する
        if (_collisionTimer.UpdateTimer())
        {
            _collider.enabled = true;
        }
    }

    /// <summary>
    /// 衝突したときに処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 機体に衝突したらダメージを与える
        if (other.CompareTag("MsCollision"))
        {
            MsDamageCollision ms = other.GetComponent<MsDamageCollision>();
            // 同じチームなので当たらない
            if (team == ms.team)
            {
                return;
            }
            if (!ms.Damage(atk, down, transform.position))
            {
                return;
            }
            if (_isHitDestroy) Destroy(gameObject);
        }

        // 建物に衝突したら自身を破壊
        if (other.CompareTag("Ground") || other.CompareTag("Building"))
        {
            Destroy(gameObject);
        }
        // エフェクトを生成
        if (other.CompareTag("MsCollision") || (other.CompareTag("Ground") || other.CompareTag("Building")))
        {
            CreteHitEffect();
            if (_seHit)
            {
                GetComponent<AudioSource>().PlayOneShot(_seHit);
            }
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
