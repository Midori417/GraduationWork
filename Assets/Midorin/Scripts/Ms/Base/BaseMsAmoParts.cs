using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsAmoParts : BaseMsParts
{
    // 現在の弾
    public int amo
    {
        get;
        protected set;
    }

    [SerializeField, Header("最大弾")]
    protected int amoMax;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Initalize()
    {
        base.Initalize();

        mainMs.AddAmoParts(this);
    }
}
