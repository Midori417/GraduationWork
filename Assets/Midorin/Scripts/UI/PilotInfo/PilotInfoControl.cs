using System;
using UnityEngine;
using UnityEngine.UI;

public class PilotInfoControl : MonoBehaviour
{
    enum State
    {
        None,
        Red,
        Blue
    }
    [Header("情報背景")]
    private Image _imgBack;

    [SerializeField, Header("情報背景画像")]
    private Sprite[] _backSprite;

    [SerializeField, Header("未選択カラー")]
    private Color _noSelectColor = Color.grey;

    [Serializable]
    private struct Info
    {
        [SerializeField, Header("タイトル")]
        public Text _title;

        [SerializeField, Header("変更テキスト")]
        public Text _select;

        [SerializeField, Header("変更")]
        public Text _txtSelect;
    }
    [SerializeField, Header("チーム")]
    private Info _team;

    [SerializeField, Header("プレイヤー")]
    private Info _player;

    [SerializeField, Header("CPU LEVEL")]
    private Info _cpuLevel;

    [Serializable]
    private struct MachineInfo
    {
        [SerializeField, Header("タイトル")]
        public Text _title;

        [SerializeField, Header("画像")]
        public Image _img;
    }
    [SerializeField, Header("マシン")]
    private MachineInfo _machine;

    /// <summary>
    /// 生成時に呼び出される
    /// </summary>
    private void Awake()
    {
        _imgBack = GetComponent<Image>();
    }

    /// <summary>
    /// Updateの前に呼び出される
    /// </summary>
    private void Start()
    {
        SetRed();
    }

    /// <summary>
    /// None状態に設定
    /// </summary>
    private void SetNone()
    {
        _imgBack.sprite = _backSprite[(int)State.None];
        _team._select.text = "出撃停止";
        _team._txtSelect.text = "<                            >";

        _player._title.color = _noSelectColor;
        _player._select.text = "";
        _player._txtSelect.text = "";

        _cpuLevel._title.color = _noSelectColor;
        _cpuLevel._select.text = "";
        _cpuLevel._txtSelect.text = "";

        _machine._title.color = _noSelectColor;
    }

    /// <summary>
    /// Red状態に設定
    /// </summary>
    private void SetRed()
    {
        _imgBack.sprite = _backSprite[(int)State.Red];
        _team._select.text = "チームA";
        _team._txtSelect.text = "<                            >";

        _player._title.color = Color.white;
        _player._select.text = "CPU";
        _player._txtSelect.text = "<                            >";

        _cpuLevel._title.color = Color.white;
        _cpuLevel._select.text = "STOP";
        _cpuLevel._txtSelect.text = "<                            >";

        _machine._title.color = Color.white;
    }

    /// <summary>
    /// Blue状態に設定
    /// </summary>
    private void SetBlue()
    {
        _imgBack.sprite = _backSprite[(int)State.Blue];
        _team._select.text = "チームB";
        _team._txtSelect.text = "<                            >";

        _player._title.color = Color.white;
        _player._select.text = "CPU";
        _player._txtSelect.text = "<                            >";

        _cpuLevel._title.color = Color.white;
        _cpuLevel._select.text = "STOP";
        _cpuLevel._txtSelect.text = "<                            >";

        _machine._title.color = Color.white;
    }
}
