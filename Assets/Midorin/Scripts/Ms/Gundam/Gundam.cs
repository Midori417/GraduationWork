using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    enum State
    {
        // 通常
        Normal,

        // 死亡
        Destroy,

        // 復活
        Respon
    }
    StateMachine<State> _stateMachine = new StateMachine<State>();

    private MsMove _move;
    private GundamMainShot _mainShot;
    private GundamSubShot _subShot;
    private GundamDamage _damage;
    [SerializeField, Header("MsDamage")]
    private MsDamageCollision _msDamageCollision;

    [Serializable]
    private struct ActiveObject
    {
        [Header("ビームライフル")]
        public GameObject _beumRifle;

        [Header("バズーカ")]
        public GameObject _bazooka;

        [Header("サーベル")]
        public GameObject _sable;

        [Header("バ―ニア")]
        public GameObject _roketFire;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            BeumRifleActive(true);
            BazookaActive(false);
            SableActive(false);
            RoketFireActive(false);
        }

        /// <summary>
        /// ビームライフルの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void BeumRifleActive(bool value)
        {
            if (!_beumRifle) return;
            _beumRifle.SetActive(value);
        }

        /// <summary>
        /// バズーカの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void BazookaActive(bool value)
        {
            if (!_bazooka) return;
            _bazooka.SetActive(value);
        }

        /// <summary>
        /// サーベルの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void SableActive(bool value)
        {
            if (!_sable) return;
            _sable.SetActive(value);
        }

        /// <summary>
        /// バーニアの切り替え
        /// </summary>
        /// <param name="value"></param>
        public void RoketFireActive(bool value)
        {
            if (!_roketFire) return;
            _roketFire.SetActive(value);
        }
    }
    [SerializeField, Header("オブジェクト")]
    private ActiveObject _activeObj;

    [SerializeField, Header("破壊されてから爆発するまで")]
    private float _destroyTime = 0;

    [SerializeField, Header("爆発エフェクト")]
    private GameObject _pfbExsprosion;

    private int _beumLayer = -1;
    private int _sableLayer = -1;

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        _move = GetComponent<MsMove>();
        _mainShot = GetComponent<GundamMainShot>();
        _subShot = GetComponent<GundamSubShot>();
        _damage = GetComponent<GundamDamage>();
        SetUp();
    }

    /// <summary>
    /// Updateより前に呼び出される
    /// </summary>
    private void Start()
    {
        // レイヤー番号を取得
        _beumLayer = animator.GetLayerIndex("BeumRifleLayer");
        _sableLayer = animator.GetLayerIndex("SableLayer");
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (isStop && isDestroy) return;

        _stateMachine.UpdateState();
        AnimUpdate();
        BoostCharge();
    }

    /// <summary>
    /// Updateの後に呼び出される
    /// </summary>
    private void LateUpdate()
    {
        _stateMachine.LateUpdateState();
    }

    #endregion

    #region 状態

    /// <summary>
    /// 状態のセットアップ
    /// </summary>
    private void SetUp()
    {
        SetUpNormal();
        SetUpDestroy();
        SetUpRespon();
        _stateMachine.Setup(State.Normal);
    }

    /// <summary>
    /// 通常状態をセットアップ
    /// </summary>
    private void SetUpNormal()
    {
        State state = State.Normal;
        Action<State> enter = (prev) =>
        {
        };
        Action update = () =>
        {
            // 破壊された
            if (hp <= 0)
            {
                _stateMachine.ChangeState(State.Destroy);
            }

            // 通常アクション
            if (ActionCheck())
            {
                Move();
                SubShot();
            }

            MsDamage();
            InvisibleUpdate();
            RoketControl();
        };
        Action lateUpdate = () =>
        {
            if (ActionCheck())
            {
                MainShot();
            }
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// 破壊状態をセットアップ
    /// </summary>
    private void SetUpDestroy()
    {
        State state = State.Destroy;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            timer.ResetTimer();
            _activeObj.RoketFireActive(false);
            animator.SetTrigger("Down");
            RedMesh();
        };
        Action update = () =>
        {
            if(timer.UpdateTimer())
            {
                var obj = Instantiate(_pfbExsprosion, transform.position, Quaternion.identity);
                Destroy(obj, 4);
                gameObject.SetActive(false);
                isDestroy = true;
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);
    }

    /// <summary>
    /// 復活状態をセットアップ
    /// </summary>
    private void SetUpRespon()
    {
        State state = State.Respon;
        GameTimer timer = new GameTimer();
        Action<State> enter = (prev) =>
        {
            timer.ResetTimer(responTime);
            Initialize();
            InvisibleTimer(1);
        };
        Action update = () =>
        {
            if(timer.UpdateTimer())
            {
                _stateMachine.ChangeState(State.Normal);
            }
        };
        Action lateUpdate = () =>
        {
        };
        Action<State> exit = (next) =>
        {
        };
        _stateMachine.AddState(state, enter, update, lateUpdate, exit);

    }

    #endregion

    /// <summary>
    /// 初期化する
    /// パイロットに呼び出してもらう
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();
        if (_move)
        {
            _move.SetMainMs(this);
            _move.Initalize();
        }
        if (_mainShot)
        {
            _mainShot.SetMainMs(this);
            _mainShot.Initalize();
        }
        if (_subShot)
        {
            _subShot.SetMainMs(this);
            _subShot.Initalize();
        }
        if (_damage)
        {
            _damage.SetMainMs(this);
            _damage.Initalize();
        }
        if (_msDamageCollision)
        {
            _msDamageCollision.SetMainMs(this);
            _msDamageCollision.Initalize();
        }
        _activeObj.Initialize();
    }

    /// <summary>
    /// 復活処理
    /// パイロットに呼び出してもらう
    /// </summary>
    public override void Respon()
    {
        base.Respon();
        _stateMachine.ChangeState(State.Respon);
    }

    /// <summary>
    /// 常に更新するアニメーション変数
    /// </summary>
    private void AnimUpdate()
    {
        float speed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGround", groundCheck.isGround);
    }

    /// <summary>
    /// アクション可能かチェック
    /// </summary>
    /// <returns>
    /// trueなら可能
    /// falseなら不可
    /// </returns>
    private bool ActionCheck()
    {
        if (_damage.isDamage || _damage.isDown || _damage.isStanding)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ロケットコントロール
    /// </summary>
    private void RoketControl()
    {
        if (ActionCheck())
        {
            bool isRoket = _move.isDash || _move.isJump || _subShot.isNow;
            _activeObj.RoketFireActive(isRoket);
        }
        else
        {
            _activeObj.RoketFireActive(false);
        }
    }

    #region 機能

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (_subShot.isNow)
            return;
        _move.UpdateState();
    }

    /// <summary>
    /// メイン射撃
    /// </summary>
    private void MainShot()
    {
        // バズーカ中は不可
        if (_subShot.isNow || _move.isLanding)
            return;

        _mainShot.UpdateState();
    }

    /// <summary>
    /// サブ射撃
    /// </summary>
    private void SubShot()
    {
        if (_mainShot.isNow || _move.isLanding) return;
        _subShot.UpdateState();
        // オブジェクトの切り替え
        if (_subShot.isNow)
        {
            animator.SetLayerWeight(_beumLayer, 0);
            _activeObj.BazookaActive(true);
            _activeObj.BeumRifleActive(false);
        }
        else
        {
            _activeObj.BazookaActive(false);
            _activeObj.BeumRifleActive(true);
        }
    }

    /// <summary>
    /// ダメージ
    /// </summary>
    private void MsDamage()
    {
        _damage.UpdateState();
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="downValue"></param>
    /// <param name="bulletPos"></param>
    /// <returns></returns>
    public override bool Damage(int damage, int downValue, Vector3 bulletPos)
    {
        if (!base.Damage(damage, downValue, bulletPos))
        {
            return false;
        }
        _damage.SetState(damage, downValue, bulletPos);

        return true;
    }

    #endregion
}
