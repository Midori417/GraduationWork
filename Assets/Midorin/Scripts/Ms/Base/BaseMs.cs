using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機体の基本コンポーネント
/// </summary>
public class BaseMs : BaseGameObject
{
    private Rigidbody _rb;
    private Animator _animator;
    private SkinnedMeshRenderer _meshRenderer;
    private GroundCheck _groundCheck;

    [Serializable]
    public struct AudioValiable
    {
        [SerializeField, Header("メイン")]
        private AudioSource _main;

        [SerializeField, Header("サブ")]
        private AudioSource _sub;

        /// <summary>
        /// メインオウディオでSEを再生
        /// </summary>
        /// <param name="clip"></param>
        public void MainSe(AudioClip clip)
        {
            if (!_main) return;
            if (!clip) return;
            _main.PlayOneShot(clip);
        }

        /// <summary>
        /// サブオウディオでSEを再生
        /// </summary>
        /// <param name="clip"></param>
        public void SubSe(AudioClip clip)
        {
            if (!_sub) return;
            if (!clip) return;
            _sub.PlayOneShot(clip);
        }
    }
    [SerializeField, Header("サウンド")]
    private AudioValiable _audio;

    [SerializeField, Header("機体の真ん中")]
    private Transform _center;

    private Transform _myCamera;
    private BaseMs _targetMs;
    private List<BaseMsAmoParts> _uiArmeds = new List<BaseMsAmoParts>();
    private Team _team = Team.None;

    [SerializeField, Header("機体コスト")]
    private int _cost = 0;

    [Serializable]
    private struct Hp
    {
        [HideInInspector, Header("現在の体力")]
        public float _current;

        [SerializeField, Header("最大体力")]
        public float _max;

        // 現在のエネルギーの割合(0～1)
        public float _rate
        { get { return Mathf.Clamp01((_max - (_max - _current)) / _max); } }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialzie()
        {
            _current = _max;
        }
    }
    [SerializeField, Header("Hp変数")]
    private Hp _hp;

    [Serializable]
    private struct Boost
    {
        // エネルギーの最大量
        private static float _max = 100;

        // 現在のエネルギー量
        private float _current;

        // 現在のエネルギーの割合(0～1)
        public float _rate
        { get { return Mathf.Clamp01((_max - (_max - _current)) / _max); } }

        // チャージタイマー
        private float _chargeTimer;

        [SerializeField, Header("チャージロック時間")]
        public float _chargeLockTime;

        [SerializeField, Header("オーバーヒートしたときのチャージロック時間")]
        public float _overHeartChargeLockTime;

        // trueなら使用中
        private bool _isUse;

        /// <summary>
        /// ブーストパラメータの初期化
        /// </summary>
        public void Initialize()
        {
            _isUse = false; 
            _current = _max;
        }

        /// <summary>
        /// チャージ処理
        /// </summary>
        public void Charge()
        {
            // 使用中はチャージを行わない
            if (_isUse)
            {
                return;
            }

            // チャージタイマーが0以上あれば減らす
            if (_chargeTimer > 0)
            {
                _chargeTimer -= Time.deltaTime;
            }
            else
            {
                // エネルギーを回復する
                _current = _max;
            }

            // 値を補正
            _chargeTimer = Mathf.Clamp(_chargeTimer, 0, _overHeartChargeLockTime);
            _current = Mathf.Clamp(_current, 0, _max);
        }

        /// <summary>
        /// ブーストの消費
        /// </summary>
        /// <param name="value">消費量</param>
        public void UseBoost(float value, bool isStatic = false)
        {
            // 0以下なら処理を行わない
            if (_current <= 0)
            {
                return;
            }

            // 消費
            if (!isStatic)
            {
                _current -= value * Time.deltaTime;
            }
            else
            {
                _current -= value;
            }
            // チャージタイムを入れておく
            if (_current > 0)
            {
                _chargeTimer = _chargeLockTime;
            }
            // 0以下ならオーバーヒート状態
            else
            {
                _chargeTimer = _overHeartChargeLockTime;
            }

            // 値を補正
            _current = Mathf.Clamp(_current, 0, _max);
        }
    }
    [SerializeField, Header("ブースト変数")]
    private Boost _boost;

    [Serializable]
    private struct TargetDistace
    {
        [SerializeField, Header("赤距離")]
        public float _redHorizontal;

        [SerializeField, Header("赤距離(上)")]
        public float _redUp;

        [SerializeField, Header("赤距離(下)")]
        public float _redDown;

        [SerializeField, Header("ロック距離")]
        public float _lookOn;

        /// <summary>
        /// 赤ロック範囲計算
        /// </summary>
        /// <param name="myPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public bool RedDistance(Vector3 myPos, Vector3 targetPos)
        {
            // 上判定
            if (myPos.y + _redUp < targetPos.y) return false;
            // 下判定
            if (myPos.y - _redDown > targetPos.y) return false;

            {
                Vector2 mp = new Vector2(myPos.x, myPos.z);
                Vector2 tp = new Vector2(targetPos.x, targetPos.z);
                // 水平計算
                float distance = Vector2.Distance(mp, tp);
                if (distance > _redHorizontal)
                {
                    return false;
                }
            }

            //  範囲内
            return true;
        }

        /// <summary>
        /// レッドロック範囲か計算
        /// </summary>
        /// <param name="myPos"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public bool LookOnDistance(Vector3 myPos, Vector3 targetPos)
        {
            if (!RedDistance(myPos, targetPos)) return false;

            {
                Vector2 mp = new Vector2(myPos.x, myPos.z);
                Vector2 tp = new Vector2(targetPos.x, targetPos.z);
                // 水平計算
                float distance = Vector2.Distance(mp, tp);
                if (distance > _lookOn)
                {
                    return false;
                }
            }

            return true;

        }
    }
    [SerializeField, Header("ターゲット距離")]
    private TargetDistace _targetDistace;

    [HideInInspector, Header("機体入力")]
    private GameInput _msInput;

    [SerializeField, Header("無敵タイマー")]
    private GameTimer _invisibleTimer = new GameTimer();

    [SerializeField, Header("復活時間")]
    private float _responTime = 1;

    // trueの時無敵タイマーを更新
    private bool _isInvisible = false;

    // ダウン値
    private float _downValue = 0;

    // trueならダメージを食らえる
    private bool _isDamageOk = true;

    // trueなら破壊が完了した
    private bool _isDestroy = false;

    // 破壊速度
    private Vector3 _destroySpeed = Vector3.zero;

    #region プロパティ

    public Rigidbody rb => _rb;
    public Animator animator => _animator;
    public SkinnedMeshRenderer meshRenderer => _meshRenderer;
    public GroundCheck groundCheck => _groundCheck;
    public AudioValiable audio => _audio;
    public Team team
    {
        get => _team;
        set => _team = value;
    }
    public Transform myCamera
    {
        get => _myCamera;
        set => _myCamera = value;
    }
    public BaseMs targetMs
    {
        get => _targetMs;
        set => _targetMs = value;
    }
    public Transform center => _center;
    public int cost => _cost;
    public float hp => _hp._current;
    public float hpMax => _hp._max;
    public float hpRate => _hp._rate;
    public bool isRedDistance
    {
        get
        {
            if (!targetMs)
            {
                return false;
            }
            // 黄色ロックなので不可
            if (!targetMs.isDamageOk)
            {
                return false;
            }
            return _targetDistace.RedDistance(center.position, targetMs.center.position);
        }
    }
    public bool isLookDistance
    {
        get
        {
            if (!targetMs)
            {
                return false;
            }
            return _targetDistace.LookOnDistance(center.position, targetMs.center.position);
        }
    }
    public float targetDistance
    {
        get
        {
            if (targetMs)
            {
                return Vector3.Distance(center.position, targetMs.center.position);
            }
            return 0f;
        }
    }
    protected float responTime => _responTime;
    public bool isDamageOk
    {
        get => _isDamageOk;
        set => _isDamageOk = value;
    }
    public bool isDestroy
    {
        get => _isDestroy;
        protected set => _isDestroy = value;
    }
    public float downValue
    {
        get => _downValue;
        set => _downValue = value;
    }
    public float boostRate => _boost._rate;
    public GameInput msInput
    {
        get => _msInput;
        set => _msInput = value;
    }
    public bool isVisible => _meshRenderer.isVisible;
    public int amoCount => _uiArmeds.Count;
    public Vector3 destroySpeed
    {
        set => _destroySpeed = value; 
    }

    #endregion

    /// <summary>
    /// 処理を開始
    /// </summary>
    public override void Play()
    {
        base.Play();
        animator.speed = 1;
        rb.useGravity = true;
        if (hp <= 0)
        {
            rb.AddForce(_destroySpeed, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 処理を停止
    /// </summary>
    public override void Stop()
    {
        base.Stop();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        _animator.speed = 0;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        if (!_rb) _rb = GetComponent<Rigidbody>();
        if (!_animator) _animator = GetComponent<Animator>();
        if (!_groundCheck) _groundCheck = GetComponentInChildren<GroundCheck>();
        if (!meshRenderer)
        {
            _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            // メッシュレンダラーマテリアルを複製
            Material[] mats = new Material[meshRenderer.materials.Length];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                mats[i] = new Material(meshRenderer.materials[i]);
                meshRenderer.materials[i] = mats[i];
            }
        }
        _hp.Initialzie();
        _boost.Initialize();
        _downValue = 0;
        _isDestroy = false;
        NormalMesh();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// メッシュ赤発光
    /// </summary>
    public void RedMesh()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    /// <summary>
    /// メッシュを元に戻す
    /// </summary>
    public void NormalMesh()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    /// <summary>
    /// ダメージを与える
    /// </summary>
    /// <param name="damage"></param>
    public virtual bool Damage(float damage, float downValue, Vector3 bulletPos, float hitStop = 0)
    {
        if (!_isDamageOk) return false;

        _hp._current -= damage;
        _hp._current = Mathf.Clamp(_hp._current, 0, hpMax);
        this._downValue += downValue;
        if (_downValue > 5)
        {
            _isDamageOk = false;
        }
        return true;
    }

    /// <summary>
    /// 無敵タイマー起動
    /// </summary>
    /// <param name="_time"></param>
    public void InvisibleTimer(float _time)
    {
        _invisibleTimer.ResetTimer(_time);
        _isInvisible = true;
    }

    /// <summary>
    /// 復活させる
    /// パイロットが呼ぶ
    /// </summary>
    public virtual void Respon()
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 無敵状態の更新
    /// </summary>
    protected void InvisibleUpdate()
    {
        if (!_isInvisible) return;

        if (_invisibleTimer.UpdateTimer())
        {
            _isInvisible = false;
            _isDamageOk = true;
        }
    }

    #region ブースト関係

    /// <summary>
    /// ブーストエネルギーのチャージ
    /// </summary>
    protected void BoostCharge()
    {
        if (!groundCheck.isGround)
        {
            return;
        }
        _boost.Charge();
    }

    /// <summary>
    /// ブーストの消費
    /// </summary>
    /// <param name="value">消費量</param>
    public void UseBoost(float value, bool isStatic = false)
    {
        _boost.UseBoost(value, isStatic);
    }

    #endregion


    /// <summary>
    /// UIに表示するパーツを追加
    /// </summary>
    /// <param name="amoParts"></param>
    public void AddAmoParts(BaseMsAmoParts amoParts)
    {
        _uiArmeds.Add(amoParts);
    }

    /// <summary>
    /// indexに対応した弾数を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetAmo(int index)
    {
        return _uiArmeds[index].amo;
    }

    /// <summary>
    /// indexに対応した弾の割合を取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetAmoRate(int index)
    {
        float amo = _uiArmeds[index].amo;
        float max = _uiArmeds[index].amoMax;
        float rate = Mathf.Clamp01((max - (max - amo)) / max);
        return rate;
    }
}
