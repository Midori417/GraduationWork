using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsAmoParts : BaseMsParts
{
    // 現在の弾
    private int _amo;

    [SerializeField, Header("最大弾")]
    private int _amoMax;

    private bool isAdd = false;

    public int amoMax => _amoMax;
    public int amo
    {
        get => _amo;
        protected set => _amo = value;
    }

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
