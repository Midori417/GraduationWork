using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : BaseAttackCollision
{
    [SerializeField, Header("衝突時のエフェクト")]
    private GameObject _pfbEffHit = null;

    [SerializeField, Header("生成してからエフェクト消すまで")]
    private float _hitEffDestroyTimer = 0;

    // trueなら衝突可能
    private bool _isCollision = false;

    // trueなら当たった
    private bool _isHit = false;

    [SerializeField, Header("Trail")]
    private TrailRenderer _trail;

    // ヒットストップ用
    private BaseMs _mainMs = null;

    #region プロパティ

    public bool isCollision
    {
        get => _isCollision;
        set
        {
            _isCollision = value;
            _trail.enabled = value;
        }
    }

    public BaseMs mainMs
    {
        get => _mainMs;
        set => _mainMs = value;
    }
    #endregion

    #region イベント関数

    /// <summary>
    /// 衝突したときに処理
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (!_isCollision || _isHit) return;

        // 機体に衝突したらダメージを与える
        if (other.CompareTag("MsCollision"))
        {
            MsDamageCollision ms = other.GetComponent<MsDamageCollision>();
            // 同じチームなので当たらない
            if (_mainMs.team == ms.team)
            {
                return;
            }

            ms.Damage(atk, down, transform.position, 1);
            _isHit = true;
            CreteHitEffect();
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

    /// <summary>
    /// 終了時に呼び出してもらう
    /// </summary>
    public void End()
    {
        _isHit = false;
    }

    /// <summary>
    /// 攻撃力とダウン値を設定
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="down"></param>
    public void SetAtkDown(int atk, float down)
    {
        this.atk = atk;
        this.down = down;
    }
}
