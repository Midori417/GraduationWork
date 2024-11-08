using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パイロットの基底クラス
/// </summary>
public class BasePilot : MonoBehaviour
{
    [SerializeField, Header("自身の機体")]
    private BaseMS myMs;

    [SerializeField, Header("自身のカメラ")]
    private GameObject myCamera;

    [SerializeField, Header("自身のUI")]
    private UIManager myUImanager;

    [SerializeField, Header("相手チームのパイロット")]
    private BasePilot[] enemyPilots;

    [SerializeField, Header("ターゲットパイロット")]
    private BasePilot targetPilot;

    [System.Serializable]
    public struct MsInput
    {
        [Header("移動入力")]
        public Vector2 moveAxis;

        [Header("ジャンプ入力")]
        public bool jumpBtn;

        [Header("ダッシュ入力")]
        public bool dashBtn;
    }

}
