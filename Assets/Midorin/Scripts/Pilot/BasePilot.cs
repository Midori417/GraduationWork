using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : BaseGameObject
{
    // 自身の操る機体
    private BaseMs _myMs;

    public BaseMs myMs
    {
        get {  return _myMs; }
        set { _myMs = value; }
    }
}
