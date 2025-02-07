using UnityEngine;

/// <summary>
/// 地面チェックコンポーネント
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [SerializeField, Header("地面を判定するタイプ")]
    private string[] groundTypes;

    // trueなら地面についている
    public bool isGround
    {
        get;
        private set;
    }

    /// <summary>
    /// 前回のフレーム着地しているか
    /// </summary>
    public bool oldIsGround
    {
        get;
        private set;
    }

    private void LateUpdate()
    {
        oldIsGround = isGround;
    }

    /// <summary>
    /// 触れたときに呼び出す
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        // 地面についている状態なので処理を行わない
        if (isGround) return;

        foreach (string type in groundTypes)
        {
            if (other.tag == type)
            {
                isGround = true;
            }
        }
    }

    /// <summary>
    /// 離れたときに呼び出す
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // 地面についていない状態なので処理を行わない
        if (!isGround) return;

        foreach (string type in groundTypes)
        {
            if (other.tag == type)
            {
                isGround = false;
            }
        }
    }
}
