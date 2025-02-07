using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// -------------------------------------------------------------
// ※使用不可
// -------------------------------------------------------------

public class MainCamera : MonoBehaviour
{
    /// <summary> プレイヤーの中心 </summary>
    [Header("カメラが追従するプレイヤーの中心")]
    [SerializeField] private Transform playerCenter;

    /// <summary> カメラコンポーネント </summary>
    private Camera mainCamera;

    /// <summary> プレイヤーまでの距離ベクトル </summary>
    private Vector3 difference;

    /// <summary> ステージ用レイヤー </summary>
    private int stageLayer;

    // Start is called before the first frame update
    void Start()
    {
        // カメラコンポーネントの取得
        mainCamera = GetComponent<Camera>();

        // ステージ用レイヤーの取得
        stageLayer = LayerMask.NameToLayer("Stage");

        // カメラコンポーネントがない場合はエラーを出す
        if (!mainCamera)
        {
            Debug.LogError("カメラコンポーネントがありません。\n" +
                "このスクリプトはカメラコンポーネントが追加されているオブジェクトに追加してください。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // カメラコンポーネントがない場合は何もしない
        if (!mainCamera)
        {
            return;
        }

        // プレイヤーまでの距離ベクトル
        difference = playerCenter.position - transform.position;

        // プレイヤーまでの方向
        Vector3 direction = difference.normalized;

        // プレイヤーに向かうRay
        Ray ray = new Ray(transform.position, direction);

        // 全ての障害物の情報
        // カメラとプレイヤーの間でRayの衝突判定を行うことで
        // カメラとプレイヤーの間に障害物がないか調べる
        RaycastHit[] hitInfoList = Physics.RaycastAll(ray, difference.magnitude);

        // 障害物のレイヤーを調べる
        foreach (var hitInfo in hitInfoList)
        {
            // 障害物にステージが含まれるなら
            // ステージを非表示にして終了する
            if (hitInfo.transform.gameObject.layer == stageLayer)
            {
                mainCamera.cullingMask &= ~(1 << stageLayer);
                break;
            }

            // ステージが含まれない時にステージが表示されるようにする
            mainCamera.cullingMask |= (1 << stageLayer);
        }
    }
}
