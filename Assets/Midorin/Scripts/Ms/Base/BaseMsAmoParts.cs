using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsAmoParts : BaseMsParts
{
    // ���݂̒e
    public int amo
    {
        get;
        protected set;
    }

    [SerializeField, Header("�ő�e")]
    protected int amoMax;

    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    public override bool Initalize()
    {
        if (!base.Initalize())
        {
            return false;
        }
        mainMs.uiArmed.Add(this);

        return true;
    }
}
