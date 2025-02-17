using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// ガンダムメイン射撃
/// </summary>
public class GundamMainShot : BaseMsAmoParts
{
    enum State
    {
        None,

        // 射撃前
        ShotF,

        // 射撃後
        ShotB,
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    [SerializeField, Header("弾Prefab")]
    private BasicBulletMove _pfbBullet = null;

    [SerializeField, Header("マズルフラッシュ")]
    private ParticleSystem _psMuzzle = null;

    [SerializeField, Header("行動を始めてから射撃するまでの時間")]
    private float _shotTime = 0;

    [SerializeField, Header("射撃してから行動を終了するまでの時間")]
    private float _endTime = 0;

    [SerializeField, Header("弾生成位置")]
    private Vector3 _shotPos = Vector3.zero;

    [SerializeField, Header("インターバル")]
    private GameTimer _interval = new GameTimer();

    [SerializeField, Header("一発の弾がリロードされるまでの時間")]
    private GameTimer _reloadTime = new GameTimer();

    [Serializable]
    private struct SpineRotValiable
    {
        [HideInInspector, Header("初期回転")]
        public Quaternion _initial;

        [HideInInspector, Header("前フレーム時の回転")]
        public Quaternion _old;

        [SerializeField, Header("回転速度")]
        public float _rotSpeed;

        [SerializeField, Header("ボーン")]
        public Transform _bone;
    }
    [SerializeField, Header("腹回転変数")]
    private SpineRotValiable _spine;

    private Transform _target;

    [SerializeField, Header("発射音")]
    private AudioClip _seShot;

    // trueならバックショット
    private bool isBack = false;

    private int _layer = -1;

    private bool _isNow = false;

    public bool isNow => _isNow;


    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        if (_psMuzzle)
            _psMuzzle.Stop();
        SetUp();
    }

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    private void Start()
    {
        // レイヤー番号を取得
        _layer = animator.GetLayerIndex("BeumRifleLayer");
    }

    #endregion

    #region 状態

    /// <summary>
    /// 状態をセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNone();
        SetUpShotF();
        SetUpShotB();
        _stateMachine.SetUp(State.None);
    }

    /// <summary>
    /// None状態をセットアップ
    /// </summary>
    private void SetUpNone()
    {
        State state = State.None;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            if (_interval.UpdateTimer())
            {
                if (msInput.GetInputDown(GameInputState.MainShot) && amo > 0)
                {
                    _stateMachine.ChangeState(State.ShotF);
                }
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// ShotF状態をセットアップ
    /// </summary>
    private void SetUpShotF()
    {
        State state = State.ShotF;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            if (_psMuzzle)
                _psMuzzle.Play();
            _isNow = true;
            timer.ResetTimer(_shotTime);
            // ターゲットを設定
            _target = targetMsCenter;
            _spine._old = _spine._bone.localRotation;
            // バックショットか判定
            if (_target)
            {
                // ターゲット方向を計算
                Vector3 directioToTarget = transform.position - _target.position;
                float dot = Vector3.Dot(directioToTarget.normalized, transform.forward);
                if (dot > 0.5f) isBack = true;
            }

            // アニメーションの選択
            if (!isBack)
            {
                animator.SetLayerWeight(_layer, 1);
                animator.SetInteger("ShotType", 0);
                animator.SetTrigger("Shot");
            }
            else
            {
                animator.SetInteger("ShotType", 1);
                animator.SetTrigger("Shot");
            }

            if (_psMuzzle)
            {
                     _psMuzzle.Play();
            }
        };
        Action update = () =>
        {
            if (!isBack)
                SpineLookRot();
            else
                LookBackRota();

            // 射撃時間になったら弾を生成
            if (timer.UpdateTimer())
            {
                CreateBullet();
                _stateMachine.ChangeState(State.ShotB);
            }
        };
        Action<State> exit = (next) =>
        {
            _spine._old = _spine._bone.localRotation;
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    /// <summary>
    /// ShotB状態をセットアップ
    /// </summary>
    private void SetUpShotB()
    {
        State state = State.ShotB;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            timer.ResetTimer(_endTime);
        };
        Action update = () =>
        {
            if (!isBack)
            {
                Quaternion smoothRotation = Quaternion.Slerp(_spine._old, _spine._initial, _spine._rotSpeed * Time.deltaTime);
                _spine._bone.localRotation = smoothRotation;
                _spine._old = _spine._bone.localRotation;
            }
            // 射撃状態を終了
            if (timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.None);
            }
        };
        Action<State> exit = (next) =>
        {
            _isNow = false;
            animator.SetLayerWeight(_layer, 0);
            _target = null;
            isBack = false;
            _interval.ResetTimer();
        };
        _stateMachine.AddState(state, enter, update, exit);
    }

    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();
        if (_spine._bone)
        {
            _spine._initial = _spine._bone.localRotation;
        }
        amo = amoMax;
        _stateMachine.ChangeState(State.None);
    }

    /// <summary>
    /// 状態を更新
    /// </summary>
    public void UpdateState()
    {
        Reload();
        _stateMachine.UpdateState();
    }

    /// <summary>
    /// 弾を生成
    /// </summary>
    private void CreateBullet()
    {
        if (!_pfbBullet) return;
        if (_target)
        {
            // ターゲットの方向を計算
            Vector3 directionToTarget = _target.position - mainMs.center.position;
            Quaternion rot = Quaternion.LookRotation(directionToTarget);
            // 弾の生成位置を計算
            Vector3 pos = mainMs.transform.position + rot * _shotPos;

            // 弾を生成
            BasicBulletMove bullet = Instantiate(_pfbBullet, pos, rot);
            bullet.team = mainMs.team;
            if (mainMs.isRedDistance)
            {
                bullet.target = _target;
            }
        }
        else
        {
            // 自身の機体のセンターを代入
            Transform center = mainMs.center;
            Quaternion rot = transform.rotation;
            Vector3 pos = center.position + rot * _shotPos;
            BasicBulletMove bullet = Instantiate(_pfbBullet, pos, rot);
            bullet.team = mainMs.team;
        }
        audio.MainSe(_seShot);
        // 弾を減らす
        amo--;
    }

    /// <summary>
    /// リロード処理
    /// </summary>
    private void Reload()
    {
        if (amo >= amoMax)
            return;

        if (_reloadTime.UpdateTimer())
        {
            _reloadTime.ResetTimer();
            amo++;
        }
    }

    #region 回転関係

    /// <summary>
    /// ターゲットの方向に向ける
    /// </summary>
    private void SpineLookRot()
    {
        if (!_target) return;
        Vector3 directionToTarget = _target.position - _spine._bone.position;
        Vector3 localDirection = _spine._bone.parent.InverseTransformDirection(directionToTarget);

        // ターゲット方向の回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(localDirection);

        // 現在の回転からターゲット回転への補完
        Quaternion smoothRotation = Quaternion.Slerp(_spine._old, targetRotation, _spine._rotSpeed * Time.deltaTime);

        // 回転を適用
        _spine._bone.localRotation = smoothRotation;
        _spine._old = _spine._bone.localRotation;
    }

    /// <summary>
    /// ターゲットの逆方向に向ける
    /// </summary>
    private void LookBackRota()
    {
        if (!_target) return;
        // ターゲット方向の回転を計算
        Vector3 directionToTarget = Vector3.Scale(transform.position - _target.position, new Vector3(1, 0, 1));
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = targetRotation;
        rb.velocity = Vector3.zero;
    }

    #endregion
}
