using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsAmoParts : BaseMsParts
{
    // 現在の弾
    private int _amo;

    [SerializeField, Header("最大弾")]
    private int _amoMax;

    // trueだとメイン機体に追加されている
    private bool isAdd = false;

    #region プロパティ

    public int amoMax => _amoMax;
    public int amo
    {
        get => _amo;
        protected set => _amo = value;
    }

    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();

        if (!isAdd)
        {
            isAdd = true;
            mainMs.AddAmoParts(this);
        }
    }
}
