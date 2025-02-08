using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理クラス
/// </summary>
public class UIManager : BaseGameObject
{
    [SerializeField, Header("自身の機体の体力")]
    private TextMeshProUGUI _txtHp;

    [SerializeField, Header("ブーストゲージ")]
    private Image _imgBoostGauge;

    [SerializeField, Header("ブーストゲージオーバーヒート")]
    private Image _imgBoostOverHeat;

    [SerializeField, Header("残り時間")]
    private TextMeshProUGUI _txtTime;

    [Serializable]
    private struct Armed
    {
        [Header("武装背景")]
        public List<Image> _imgBack;

        [Header("武装の弾")]
        public List<TextMeshProUGUI> _txtValue;

        [Header("武装ゲージ")]
        public List<Image> _imgGauge;
    }
    [SerializeField, Header("武装変数")]
    private Armed _armed;

    [Serializable]
    private struct TargetMark
    {
        [Header("ターゲットImage")]
        public Image _img;

        [Header("赤カーソル")]
        public Sprite _red;

        [Header("ロックオンカーソル")]
        public Sprite _lookOn;

        [Header("緑カーソル")]
        public Sprite _green;

        [Header("イエローカーソル")]
        public Sprite _yellow;
    }
    [SerializeField, Header("ターゲットカーソル")]
    private TargetMark _targetMark;

    [Serializable]
    private struct EnemyHp
    {
        [Header("体力バー")]
        public List<Image> _bar;

        [Header("背景")]
        public List<Image> _back;
    }
    [SerializeField, Header("エネミーHp")]
    private EnemyHp _enemyHp;

    [Serializable]
    private struct TeamHp
    {
        [Header("体力バー")]
        public Image _bar;
        [Header("背景")]
        public Image _barBack;

        [Header("体力")]
        public TextMeshProUGUI _hp;

        [Header("体力背景")]
        public Image _hpBack;
    }
    [SerializeField, Header("チームHp")]
    private TeamHp _teamHp;

    [Serializable]
    private struct StrengthGauge
    {
        [Header("戦力ゲージ0味方1敵")]
        public List<Image> _imgGauge;

        [Header("戦力ゲージWarning")]
        public List<Image> _imgWarning;
    }
    [SerializeField, Header("戦力ゲージ")]
    private StrengthGauge _strengthGauge;

    [Serializable]
    private struct EventUI
    {
        [Header("イベントUI(大)")]
        public Image _img;

        [Header("イベントUI(小)")]
        public Image _imgSmall;

        [Header("勝利画像(大)")]
        public Sprite _winLarge;

        [Header("勝利画像(小)")]
        public Sprite _winSmall;

        [Header("敗北画像(大)")]
        public Sprite _loseLarge;

        [Header("敗北画像(小)")]
        public Sprite _loseSmall;

        [Header("引き分け画像(大)")]
        public Sprite _drawLarge;

        [Header("引き分け画像(小)")]
        public Sprite _drawSmall;

        #region 画像関数

        /// <summary>
        /// 大きい画像切り替え
        /// </summary>
        public void Big(Victory victory)
        {
            if (!_img) return;
            _img.enabled = true;
            switch (victory)
            {
                case Victory.Win:
                  _img.sprite = _winLarge;
                    break;
                case Victory.Lose:
                    _img.sprite = _loseLarge;
                    break;
                case Victory.Draw:
                    _img.sprite = _drawLarge;
                    break;
            }
        }

        /// <summary>
        /// 小さい画像切り替え
        /// </summary>
        public void Small(Victory victory)
        {
            if (!_imgSmall) return;
            _img.enabled = false;
            _imgSmall.enabled = true;
            switch (victory)
            {
                case Victory.Win:
                    _imgSmall.sprite = _winSmall;
                    break;
                case Victory.Lose:
                    _imgSmall.sprite = _loseSmall;
                    break;
                case Victory.Draw:
                    _imgSmall.sprite = _drawSmall;
                    break;
            }
        }

        #endregion
    }
    [SerializeField, Header("イベントUI")]
    private EventUI _eventUI;

    private BattleManager _battleManager;

    [SerializeField, Header("プレイヤーキャンバス")]
    private GameObject _playerUI;

    GameTimer _hpFlashTimer = new GameTimer(0.2f);
    bool _isHpFlash = false; 

    #region イベント関数

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
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
        if (_hpFlashTimer.UpdateTimer())
        {
            _hpFlashTimer.ResetTimer();
            _isHpFlash = !_isHpFlash;
        }
    }

    #endregion

    /// <summary>
    /// カラーをカラーコードに変換
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string ColorToHex(Color color)
    {
        return UnityEngine.ColorUtility.ToHtmlStringRGB(color);
    }

    /// <summary>
    /// 数字をテキストに変換
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string GetTextNum(float num, Color color, bool f2 = false)
    {
        string numStr = "";
        if (f2)
        {
            numStr = Mathf.Abs(num).ToString("F2");
        }
        else
        {
            numStr = Mathf.Abs(num).ToString();
        }
        // テキストに変換していく
        string str = "";
        for (int i = 0; i < numStr.Length; i++)
        {
            if (numStr[i] != '.')
            {
                str += "<sprite=" + numStr[i] + " color=#" + ColorToHex(color) + ">";
            }
            else
            {
                str += "<sprite=10>";
            }
        }
        return str;
    }

    #region UI操作

    /// <summary>
    /// タイマーの設定
    /// </summary>
    private void Timer()
    {
        if (!_battleManager) return;
        //_txtTime.text = _battleManager.battleTimer.ToString("f2");
        _txtTime.text = GetTextNum(_battleManager.battleTimer, Color.white, true);
    }

    /// <summary>
    /// ブーストゲージの設定
    /// </summary>
    /// <param name="value">現在のブースト容量(0～1)</param>
    public void BoostGauge(float value)
    {
        if (_imgBoostGauge)
        {
            if(value < 0.2f)
            {
                _imgBoostGauge.color = Color.red;
            }
            else if(value < 0.5f)
            {
                _imgBoostGauge.color = Color.yellow;
            }
            else
            {
                _imgBoostGauge.color = new Color(0, 0.7f, 1);
            }
            _imgBoostGauge.fillAmount = value;
        }
        if(value <= 0)
        {
            _imgBoostOverHeat.enabled = true;
        }
        else
        {
            _imgBoostOverHeat.enabled = false;
        }
    }

    /// <summary>
    /// 武装の残弾を設定
    /// </summary>
    /// <param name="value"></param>
    public void ArmedValue(int index, int _value, float rate)
    {
        if (!_armed._txtValue[index] && !_armed._imgBack[index] && !_armed._imgGauge[index])
        {
            return;
        }
        // 表示されていなければ表示
        if (!_armed._imgBack[index].IsActive())
        {
            _armed._imgBack[index].gameObject.SetActive(true);
        }

        // 弾を設定
        if (_armed._txtValue[index])
        {
            Color color = Color.white;
            if (_value <= 0)
            {
                color = Color.red;
            }
            else
            {
                color = Color.white;
            }
            string str = GetTextNum(_value, color);
            _armed._txtValue[index].text = str;
        }
        // ゲージを設定
        if (_armed._imgGauge[index])
        {
            _armed._imgGauge[index].fillAmount = rate;
        }
    }

    /// <summary>
    /// 戦力値設定
    /// </summary>
    /// <param name="teamId"></param>
    public void SetStrengthGauge(Team teamId)
    {
        if (!_strengthGauge._imgGauge[0] || !_strengthGauge._imgGauge[1])
            return;
        if (!_battleManager) return;
        float max = GameManager.teamCostMax;
        float red = (max - (max - _battleManager.redCost)) / max;
        float blue = (max - (max - _battleManager.blueCost)) / max;
        if (teamId == Team.Red)
        {
            _strengthGauge._imgGauge[0].fillAmount = red;
            _strengthGauge._imgGauge[1].fillAmount = blue;
            if(red < 0.5)
            {
                _strengthGauge._imgWarning[0].enabled = true;
            }
            if (blue < 0.5)
            {
                _strengthGauge._imgWarning[1].enabled = true;
            }
        }
        else if (teamId == Team.Blue)
        {
            _strengthGauge._imgGauge[0].fillAmount = blue;
            _strengthGauge._imgGauge[1].fillAmount = red;
            if (red < 0.5)
            {
                _strengthGauge._imgWarning[0].enabled = true;
            }
            if (blue < 0.5)
            {
                _strengthGauge._imgWarning[0].enabled = true;
            }
        }
    }

    /// <summary>
    /// 体力の設定
    /// </summary>
    /// <param name="current"></param>
    public void SetHp(int current, float rate)
    {
        if (_txtHp)
        {
            Color color = Color.white;
            if (rate <= 0.3)
            {
                if(_isHpFlash)
                {
                    color = Color.red;
                }
                else
                {
                    color = new Color(0.3f, 0.0f, 0.0f);
                }
            }
            else if (rate <= 0.5f)
            {
                color = Color.yellow;
            }
            else
            {
                color = Color.white;
            }
            _txtHp.text = GetTextNum(current, color);
        }
    }

    /// <summary>
    /// 勝敗を設定
    /// </summary>
    /// <param name="victory"></param>
    public void SetVictory(Victory victory, bool isEnd)
    {
        if (!isEnd)
        {
            _eventUI.Big(victory);
        }
        else
        {
            _eventUI.Small(victory);
        }
    }

    /// <summary>
    /// ターゲットカーソルを設定
    /// </summary>
    /// <param name="type"></param>
    public void SetTargetMark(TargetType type, Vector3 pos)
    {
        switch (type)
        {
            case TargetType.Red:
                _targetMark._img.sprite = _targetMark._red;
                break;
            case TargetType.Green:
                _targetMark._img.sprite = _targetMark._green;
                break;
            case TargetType.Yellow:
                _targetMark._img.sprite = _targetMark._yellow;
                break;
            case TargetType.LookOn:
                _targetMark._img.sprite = _targetMark._lookOn;
                break;
        }

        _targetMark._img.rectTransform.position = pos;
    }

    /// <summary>
    /// ターゲット体力を設定
    /// </summary>
    /// <param name="hpRate"></param>
    public void SetEnemHp(int index, float hpRate, Vector3 pos, bool isEnable)
    {
        if (!_enemyHp._bar[index] || !_enemyHp._back[index]) return;

        // 体力がゼロ以下または画面外なら非表示
        if (hpRate <= 0)
        {
            if (_enemyHp._back[index].gameObject.activeSelf)
            {
                _enemyHp._back[index].gameObject.SetActive(false);
            }
        }
        else
        {
            if (isEnable)
            {
                if (!_enemyHp._back[index].gameObject.activeSelf)
                {
                    _enemyHp._back[index].gameObject.SetActive(true);
                }
            }
            else
            {
                if (_enemyHp._back[index].gameObject.activeSelf)
                {
                    _enemyHp._back[index].gameObject.SetActive(false);
                }
            }
        }
        _enemyHp._bar[index].fillAmount = hpRate;

        _enemyHp._back[index].rectTransform.position = pos + new Vector3(162, 85, 0);
    }

    /// <summary>
    /// ターゲット体力を設定
    /// </summary>
    /// <param name="hpRate"></param>
    public void SetPartnerHpBar(float hpRate, Vector3 pos, bool isEnable)
    {
        if (!_teamHp._bar || !_teamHp._barBack) return;

        // 体力がゼロ以下または画面外なら非表示
        if (hpRate <= 0)
        {
            if (_teamHp._barBack.gameObject.activeSelf)
            {
                _teamHp._barBack.gameObject.SetActive(false);
                _teamHp._hpBack.gameObject.SetActive(false);
            }
        }
        else
        {
            if (isEnable)
            {
                if (!_teamHp._barBack.gameObject.activeSelf)
                {
                    _teamHp._barBack.gameObject.SetActive(true);
                    _teamHp._hpBack.gameObject.SetActive(true);
                }
            }
            else
            {
                if (_teamHp._barBack.gameObject.activeSelf)
                {
                    _teamHp._barBack.gameObject.SetActive(false);
                    _teamHp._hpBack.gameObject.SetActive(false);
                }
            }
        }
        _teamHp._bar.fillAmount = hpRate;

        _teamHp._barBack.rectTransform.position = pos + new Vector3(162, 85, 0);
    }

    /// <summary>
    /// パートナー体力を設定
    /// </summary>
    /// <param name="hp"></param>
    public void SetPartnerHp(float hp, float rate)
    {
        if (!_teamHp._hpBack.gameObject.activeSelf)
        {
            _teamHp._hpBack.gameObject.SetActive(true);
        }
        Color color = Color.white;
        if (rate <= 0.3f)
        {
            if (_isHpFlash)
            {
                color = Color.red;
            }
            else
            {
                color = new Color(0.3f, 0.0f, 0.0f);
            }
        }
        else if (rate <= 0.5f)
        {
            color = Color.yellow;
        }
        else
        {
            color = Color.white;
        }
        _teamHp._hp.text = GetTextNum(hp, color);
    }

    public override void Play()
    {
        base.Play();
        _playerUI.gameObject.SetActive(true);
    }

    public override void Stop()
    {
        base.Stop();
        _playerUI.gameObject.SetActive(false);
    }

    #endregion
}
