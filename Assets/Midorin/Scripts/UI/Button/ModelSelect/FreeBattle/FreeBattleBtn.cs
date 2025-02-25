using UnityEngine;

namespace ModeSelectBtn
{
    /// <summary>
    /// フリーバトルボタン
    /// </summary>
    public class FreeBattleBtn : SelectBtn
    {
        [SerializeField, Header("フェードオブジェクト")]
        private FadeOut _fadeOut;

        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 選択されたときの処理
        /// </summary>
        public override void Select()
        {
            _fadeOut.FadeStrt(Global._battleSettingScene);
        }
    }
}