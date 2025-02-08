using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// タイトル管理クラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private FadeOut _fadeOut;

    public Image targetImage;  // 点滅させるImage
    public Color startColor = Color.white; // 初期色
    public Color endColor = new Color(1, 1, 1, 0); // 点滅時の色（透明など）
    public float speed = 1.0f; // 点滅の速度

    [SerializeField]
    private Image _imgPlaseButton;

    [SerializeField, Header("画像")]
    private List<Sprite> _sprites;

    private float t = 0f;

    bool isOn = false;
    [SerializeField]
    AudioSource _bgmSource;

    void Update()
    {
        if (targetImage != null)
        {
            // Sin波で0〜1の範囲を繰り返す
            t = Mathf.PingPong(Time.time * speed, 1);
            targetImage.color = Color.Lerp(startColor, endColor, t);
        }
        if (!isOn)
        {
            if (Gamepad.current != null)
            {
                if (Cursor.visible)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                _imgPlaseButton.sprite = _sprites[0];
                if (Gamepad.current.crossButton.wasPressedThisFrame)
                {
                    isOn = true;
                    PushPleaseButton();
                    GetComponent<AudioSource>().Play();
                    _bgmSource.Stop();
                }
            }
            else
            {
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                _imgPlaseButton.sprite = _sprites[1];
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    isOn = true;
                    PushPleaseButton();
                    GetComponent<AudioSource>().Play();
                    _bgmSource.Stop();
                }
            }
        }
    }

    /// <summary>
    /// pleaseボタンを押されたときの処理
    /// </summary>
    public void PushPleaseButton()
    {
        if (!_fadeOut)
        {
            Debug.LogError("フェードオブジェクトが存在しません");
            return;
        }
        _fadeOut.FadeStrt(Global._selectScene);
    }
}
