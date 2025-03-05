using UnityEngine;

namespace FreeBattleSetting
{
    /// <summary>
    /// 
    /// </summary>
    public class BattleStartBtn : SelectBtn
    {
        [SerializeField, Header("フェードオブジェクト")]
        private FadeOut _fadeOut;

        /// <summary>
        /// 生成時に呼び出される
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 選択されたときの処理
        /// </summary>
        public override void Select()
        {
            _fadeOut.FadeStrt(Global._battleScene);
        }
    }
}