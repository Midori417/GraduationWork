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

    [SerializeField, Header("戦力0味方1敵")]
    private List<Image> _imgStrengthGauge;

    [SerializeField, Header("イベントUI")]
    private Image _imgEvent;

    [SerializeField, Header("勝利画像")]
    private Sprite _win;

    [SerializeField, Header("敗北画像")]
    private Sprite _lose;

    [SerializeField, Header("引き分け画像")]
    private Sprite _draw;

    private BattleManager _battleManager;

    [SerializeField]
    private GameObject _playerUI;

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
        _txtTime.text = _battleManager.battleTimer.ToString("f2");
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
            if (_value <= 0)
            {
                _armed._txtValue[index].color = Color.red;
            }
            else
            {
                _armed._txtValue[index].color = Color.white;
            }
            _armed._txtValue[index].text = _value.ToString();
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
    public void StrengthGauge(Team teamId)
    {
        if (!_imgStrengthGauge[0] || !_imgStrengthGauge[1])
            return;
        if (!_battleManager) return;
        float max = GameManager.teamCostMax;
        float red = (max - (max - _battleManager.redCost)) / max;
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
    /// <param name="current"></param>
    public void SetHp(int current, float rate)
    {
        if (_txtHp)
        {
            if(rate <= 0)
            {
                _txtHp.color = Color.red;
            }
            else if(rate <= 0.5f)
            {
                _txtHp.color = Color.yellow;
            }
            else
            {
                _txtHp.color = Color.white;
            }
            _txtHp.text = current.ToString();
        }
    }

    /// <summary>
    /// 勝敗を設定
    /// </summary>
    /// <param name="victory"></param>
    public void SetVictory(Victory victory)
    {
        _imgEvent.enabled = true;
        if (victory == Victory.Win)
        {
            _imgEvent.sprite = _win;
        }
        else if (victory == Victory.Lose)
        {
            _imgEvent.sprite = _lose;
        }
        else
        {
            _imgEvent.sprite = _draw;
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
            if(isEnable)
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
        if (rate <= 0)
        {
            _teamHp._hp.color = Color.red;
        }
        else if (rate <= 0.5f)
        {
            _teamHp._hp.color = Color.yellow;
        }
        else
        {
            _teamHp._hp.color = Color.white;
        }
        _teamHp._hp.text = hp.ToSafeString();
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
}
