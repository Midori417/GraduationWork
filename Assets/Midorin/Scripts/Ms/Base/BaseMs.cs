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

    [SerializeField, Header("機体の真ん中")]
    private Transform _center;

    private Transform _myCamera;
    private BaseMs _targetMs;
    private List<BaseMsAmoParts> _uiArmeds = new List<BaseMsAmoParts>();

    [Serializable]
    private struct Hp
    {
        [HideInInspector, Header("現在の体力")]
        public int _current;

        [Header("最大体力")]
        public int _max;

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

    // trueならダメージを受ける
    protected bool isDamageOk = true;

    // ダウン値
    protected float _downValue = 0;

    // trueならダウン中
    protected bool _isDown = false;

    [Serializable]
    private struct Boost
    {
        // エネルギーの最大量
        private static float _max = 100;

        // 現在のエネルギー量
        private float _current;

        // 現在のエネルギーの割合(0～1)
        public float _current01
        { get { return Mathf.Clamp01((_max - (_max - _current)) / _max); } }

        // チャージタイマー
        private float _chargeTimer;

        [Header("チャージロック時間")]
        public float _chargeLockTime;

        [Header("オーバーヒートしたときのチャージロック時間")]
        public float _overHeartChargeLockTime;

        // trueなら使用中
        private bool _isUse;

        /// <summary>
        /// ブーストパラメータの初期化
        /// </summary>
        public void Initialize()
        {
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
        public void UseBoost(float value)
        {
            // 0以下なら処理を行わない
            if (_current <= 0)
            {
                return;
            }

            // 消費
            _current -= value * Time.deltaTime;

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

    [HideInInspector]
    public MsInput _msInput;

    #region ゲッター

    public Rigidbody rb => _rb;
    public Animator animator => _animator;
    public SkinnedMeshRenderer meshRenderer => _meshRenderer;
    protected GroundCheck groundCheck => _groundCheck;

    public Transform myCamera => _myCamera;
    public BaseMs targetMs => _targetMs;
    public Transform center => _center;
    public int hp => _hp._current;
    public int hpMax => _hp._max;
    public float boost01 => _boost._current01;
    public MsInput msInput => _msInput;

    public int amoCount => _uiArmeds.Count;

    #endregion

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
    }

    /// <summary>
    /// 復活処理
    /// </summary>
    public virtual void Remove()
    {
        Initialize();
        isDamageOk = true;
        Invoke("RemoveInvincible", 2);
    }

    /// <summary>
    /// 無敵解除
    /// </summary>
    protected void RemoveInvincible()
    {
        isDamageOk = true;
    }

    /// <summary>
    /// メッシュ赤発光
    /// </summary>
    protected void MeshDamage()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    /// <summary>
    /// メッシュを元に戻す
    /// </summary>
    protected void MeshRemove()
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
    public virtual bool Damage(int damage, int _downValue, Vector3 bulletPos)
    {
        // ダメージ処理不可
        if (!isDamageOk) return false;

        _hp._current += damage;
        _hp._current = Mathf.Clamp(_hp._current, 0, hpMax);
        this._downValue += _downValue;
        return true;
    }

    /// <summary>
    /// 破壊されているかチェック
    /// </summary>
    /// <returns>
    /// true 破壊されている
    /// </returns>
    public bool DestroyCheck()
    {
        if (hp <= 0)
        {
            return true;
        }
        return false;
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
    public void UseBoost(float value)
    {
        _boost.UseBoost(value);
    }

    #endregion

    /// <summary>
    /// ターゲット機体を設定
    /// パイロットに呼び出す
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetMs(BaseMs target)
    {
        _targetMs = target;
    }

    /// <summary>
    /// 自身のカメラを設定
    /// パイロットで呼び出す
    /// </summary>
    /// <param name="myCameraTrs"></param>
    public void SetMyCamera(Transform myCameraTrs)
    {
        _myCamera = myCameraTrs;
    }

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
}
