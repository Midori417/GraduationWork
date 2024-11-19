using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MSのパーツの基底クラス
/// </summary>
public class BaseParts : MonoBehaviour
{
    // メインコンポーネント
    private BaseMs mainMs;

    /// <summary>
    /// 基底パーツに必要なコンポーネントを取得
    /// </summary>
    protected void GetBasePartsComponent()
    {
        mainMs = GetComponent<BaseMs>();
        if (!mainMs)
        {
            Debug.LogError("メインコンポネントがないよ");
        }
    }

    /// <summary>
    /// カメラの正面を基準に移動方向を計算する
    /// </summary>
    /// <returns></returns>
    protected Vector3 MoveForward(Vector2 moveAxis)
    {
        if (mainMs)
        {
            return mainMs.MoveForward(moveAxis);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// ブースト残量を取得
    /// </summary>
    protected float GetBoostCurrent()
    {
        if (mainMs)
        {
            return mainMs.boostCurrent;
        }
        return 0;
    }

   /// <summary>
   /// ブーストを使用する
   /// </summary>
   /// <param name="useBoost">使用ブースト量</param>
    protected void UseBoost(float useBoost)
    {
        if(mainMs)
        {
            mainMs.UseBoost(useBoost);
        }
    }
}
