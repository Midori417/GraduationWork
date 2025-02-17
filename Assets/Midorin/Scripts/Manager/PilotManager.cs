using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// パイロット管理クラス
/// </summary>
public class PilotManager 
{
    private struct PilotVaiable
    {
        // 全パイロット
        public List<BasePilot> _all;

        // 赤チームパイロット
        public List<BasePilot> _red;

        // 青チームパイロット
        public List<BasePilot> _blue;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _all = new List<BasePilot>();
            _red = new List<BasePilot>();
            _blue = new List<BasePilot>();
        }
    }
    private PilotVaiable _pilot;

    /// <summary>
    /// 初期化する
    /// </summary>
    public void Initialize()
    {
        _pilot.Initialize();
    }

    /// <summary>
    /// パイロットのセットアップ
    /// </summary>
    public void SetUpPilot()
    {
        // 敵チームを設定
        foreach (BasePilot pilot in _pilot._red)
        {
            pilot.enemyPilots = _pilot._blue;
        }
        foreach (BasePilot pilot in _pilot._blue)
        {
            pilot.enemyPilots = _pilot._red;
        }
        if (_pilot._red.Count > 1)
        {
            _pilot._red[0].teamPilot = _pilot._red[1];
            _pilot._red[1].teamPilot = _pilot._red[0];
        }
        if (_pilot._blue.Count > 1)
        {
            _pilot._blue[0].teamPilot = _pilot._blue[1];
            _pilot._blue[1].teamPilot = _pilot._blue[0];
        }

        // パイロットの初期化
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Initialize();
        }

        Stop();
    }

    /// <summary>
    /// 初期位置を設定
    /// </summary>
    /// <param name="_redStart"></param>
    /// <param name="_blueStart"></param>
    public void SetStartPos(Transform _redStart, Transform _blueStart)
    {
        for (int i = 0; i < _pilot._red.Count; ++i)
        {
            _pilot._red[i].myMs.transform.SetPositionAndRotation(_redStart.position, _redStart.rotation);
            _pilot._red[i].myMs.transform.Translate(30 * i, 0, 0);
        }
        for (int i = 0; i < _pilot._blue.Count; ++i)
        {
            _pilot._blue[i].myMs.transform.SetPositionAndRotation(_blueStart.position, _blueStart.rotation);
            _pilot._blue[i].myMs.transform.Translate(30 * i, 0, 0);
        }
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void End()
    {
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.End();
        }
    }

    /// <summary>
    /// 赤チーム勝利
    /// </summary>
    public void RedWin()
    {
        foreach (BasePilot pilot in _pilot._red)
        {
            pilot.SetVitory(Victory.Win);
        }
        foreach (BasePilot pilot in _pilot._blue)
        {
            pilot.SetVitory(Victory.Lose);
        }
    }

    /// <summary>
    /// 青チーム勝利
    /// </summary>
    public void BlueWin()
    {
        foreach (BasePilot pilot in _pilot._red)
        {
            pilot.SetVitory(Victory.Lose);
        }
        foreach (BasePilot pilot in _pilot._blue)
        {
            pilot.SetVitory(Victory.Win);
        }
    }

    /// <summary>
    /// 引き分け
    /// </summary>
    public void Draw()
    {
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.SetVitory(Victory.Draw);
        }
    }

    #region 再生・停止

    /// <summary>
    /// 処理をさせる
    /// </summary>
    public void Play()
    {
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Play();
        }
    }

    /// <summary>
    /// 処理を停止する
    /// </summary>
    public void Stop()
    {
        foreach (BasePilot pilot in _pilot._all)
        {
            pilot.Stop();
        }
    }

    #endregion

    #region パイロットの追加

    /// <summary>
    /// パイロットの追加
    /// </summary>
    /// <param name="pilot"></param>
    public void AddPilot(BasePilot pilot)
    {
        _pilot._all.Add(pilot);
    }

    /// <summary>
    /// 赤チームパイロットの追加
    /// </summary>
    /// <param name="pilot"></param>
    public void AddRedPilot(BasePilot pilot)
    {
        _pilot._red.Add(pilot);
        AddPilot(pilot);
    }

    /// <summary>
    /// 青チームパイロットの追加
    /// </summary>
    /// <param name="pilot"></param>
    public void BlueAddPilot(BasePilot pilot)
    {
        _pilot._blue.Add(pilot);
        AddPilot(pilot);
    }

    #endregion
}
