using UnityEngine;

namespace ModeSelectBtn
{
    /// <summary>
    /// 
    /// </summary>
    public class ExitBattleBtn : SelectBtn
    {
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 選択されたときの処理
        /// </summary>
        public override void Select()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }
}