using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ガンダム
/// </summary>
public class Gundam : BaseMs
{
    private MsMove _move;

    [Serializable]
    private class ActiveObject
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

    #region イベント関数

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        _move = GetComponent<MsMove>();
    }

    /// <summary>
    /// Updateより前に呼び出される
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        if (DestroyCheck())
        {
            // 破壊された
            return;
        }
        BoostCharge();
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
        _activeObj.Initialize();
    }
}
