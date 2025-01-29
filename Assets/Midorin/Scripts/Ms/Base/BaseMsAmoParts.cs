using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMsAmoParts : BaseMsParts
{
    // Œ»İ‚Ì’e
    public int amo
    {
        get;
        protected set;
    }

    [SerializeField, Header("Å‘å’e")]
    protected int amoMax;

    /// <summary>
    /// ‰Šú‰»
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
