using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基本の弾の動き
/// </summary>
public class BasicBulletMove : BaseGameObject
{
    /// <summary> 速度 </summary>
    [SerializeField, Header("速度")]
    private float _speed = 0;

    [SerializeField, Header("破棄時間")]
    GameTimer _destroyTimer = new GameTimer();

    /// <summary> 追尾可能な角度 </summary>
    [SerializeField, Range(0, 180.0f), Header("追尾可能な角度(度数法)")]
    private float _homingAngle = 0;

    /// <summary> 追尾性能(%) </summary>
    [SerializeField, Range(0, 100.0f), Header("追尾性能(%), 100%で追尾可能な角度分回転して追尾する")]
    private float _homingPercent = 0;

    /// <summary> %の最大値 </summary>
    private readonly float _percentMax = 100.0f;

    /// <summary> 射撃対象 </summary>
    private Transform _target = null;

    private Team _team;

    /// <summary>
    /// 射撃対象用プロパティ
    /// </summary>
    public Transform target
    {
        set { _target = value; }
    }

    public Team team
    {
        get => _team;
        set => _team = value;
    }

    #region イベント関数

    private void Awake()
    {
        BattleManager i = BattleManager.I;
        if(i)
       i.SetBullet(this);
    }

    private void OnDestroy()
    {
        BattleManager i = BattleManager.I;
        if (i)
            i.RemoveBullet(this);
    }

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    void Start()
    {
        // nullチェック
        if (!_target)
        {
            return;
        }

        //// 射撃対象がいる方向に向ける
        //transform.LookAt(_target.position);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    void Update()
    {
        if (isStop) return;

        // 破棄時間
        if (_destroyTimer.UpdateTimer())
        {
            Destroy(gameObject);
        }

        Move();
        TargetLookMove();
    }

    #endregion

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        // 向いている方向に進む
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    /// <summary>
    /// ターゲットの方向に向いての処理
    /// </summary>
    private void TargetLookMove()
    {
        if (!_target) return;

        // 射撃対象までの向きベクトル
        Vector3 direction = _target.position - transform.position;

        // 射撃対象までの角度
        float angleDiff = Vector3.Angle(transform.forward, direction);

        // 射撃対象の方向を向いたときのクォータニオン
        Quaternion rotateTarget = Quaternion.LookRotation(direction);

        // 角度が一定以下なら射撃対象に向かって回転させる
        if (angleDiff <= _homingAngle)
        {
            // 追尾性能(割合)
            float homingRatio = _homingPercent / _percentMax;

            // 追尾性能(割合)分の回転をさせる
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateTarget, homingRatio);
        }
    }
}