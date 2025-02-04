using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理クラス
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField, Header("自身の機体の体力")]
    private Text _txtHp;

    [SerializeField, Header("ブーストゲージ")]
    private Image _imgBoostGauge;

    [SerializeField, Header("残り時間")]
    private Text _txtTime;

    [SerializeField, Header("武装")]
    private List<Image> _imgArmed;

    [SerializeField, Header("武装の弾")]
    private List<Text> _txtArmedValues;

    [SerializeField, Header("武装ゲージ")]
    private List<Image> _imgArmedGauge;

    [SerializeField, Header("戦力0味方1敵")]
    private List<Image> _imgStrengthGauge;

    [SerializeField, Header("イベントUI")]
    private Image _imgEvent;

    [SerializeField, Header("勝利画像")]
    private Sprite _win;

    [SerializeField, Header("敗北画像")]
    private Sprite _lose;

    private BattleManager _battleManager;

    private void Start()
    {
        if (!_battleManager) _battleManager = BattleManager.I;
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        Timer();
    }

    /// <summary>
    /// タイマーの設定
    /// </summary>
    private void Timer()
    {
        if (!_battleManager) return;
        _txtTime.text = GameTimer.GetMinutes(_battleManager.battleTimer).ToString()
            + ":"
            + GameTimer.GetSeconds(_battleManager.battleTimer).ToString("00");
    }

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量(0～1)</param>
    public void BoostGauge(float value)
    {
        if (_imgBoostGauge)
        {
            _imgBoostGauge.fillAmount = value;
        }
    }

    /// <summary>
    /// 武装の残弾を設定
    /// </summary>
    /// <param name="value"></param>
    public void ArmedValue(int index, int _value, float rate)
    {
        if (_txtArmedValues.Count - 1 < index)
        {
            return;
        }
        if (!_imgArmed[index].IsActive())
        {
            _imgArmed[index].gameObject.SetActive(true);
        }

        if (_txtArmedValues[index])
        {
            _txtArmedValues[index].text = _value.ToString();
        }
        if(_imgArmedGauge[index])
        {
            _imgArmedGauge[index].fillAmount = rate;
        }
    }

    /// <summary>
    /// 戦力値設定
    /// </summary>
    /// <param name="teamId"></param>
    public void StrengthGauge(Team teamId)
    {
        if (!_imgStrengthGauge[0] || !_imgStrengthGauge[1])
            return;
        if (!_battleManager) return;
        float max = GameManager.teamCostMax;
        float red = (max -(max -  _battleManager.redCost)) / max;
        float blue = (max - (max - _battleManager.blueCost)) / max;
        if (teamId == Team.Red)
        {
            _imgStrengthGauge[0].fillAmount = red;
            _imgStrengthGauge[1].fillAmount = blue;
        }
        else if (teamId == Team.Blue)
        {
            _imgStrengthGauge[0].fillAmount = blue;
            _imgStrengthGauge[1].fillAmount = red;
        }
    }

    /// <summary>
    /// 体力の設定
    /// </summary>
    /// <param name="value"></param>
    public void Hp(int value)
    {
        if (_txtHp)
        {
            _txtHp.text = value.ToString();
        }
    }

    /// <summary>
    /// 勝敗を設定
    /// </summary>
    /// <param name="victory"></param>
    public void SetVictory(Victory victory)
    {
        _imgEvent.enabled = true;
        if(victory == Victory.Win)
        {
            _imgEvent.sprite = _win;
        }
        else if(victory == Victory.Lose)
        {
            _imgEvent.sprite = _lose;
        }
    }
}
