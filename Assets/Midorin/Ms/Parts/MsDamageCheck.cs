using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsDamageCheck : BaseMsParts
{
    /// <summary>
    /// ������
    /// </summary>
    /// <returns>
    /// true    ����������
    /// faklse  ���������s
    /// </returns>
    public override bool Initalize()
    {
        if (!base.Initalize())
        {
            return false;
        }

        return true;
    }

    public void Damage(int damage, int _downValue, Vector3 bulletPos)
    {
        mainMs.Damage(damage, _downValue, bulletPos);
    }
}
