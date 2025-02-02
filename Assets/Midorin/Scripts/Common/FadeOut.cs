using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// フェードアウトしてシーンを切り替える
/// </summary>
public class FadeOut : MonoBehaviour
{
    [SerializeField, Header("フェードオブジェクト")]
    private Image _img;

    [SerializeField, Header("フェードにかかる時間")]
    private float _fadeDuration = 1.0f;

    /// <summary>
    /// スタートイベント
    /// </summary>
    public void Start()
    {
        // Nullチェック
        if (!_img)
        {
            _img = GetComponent<Image>();
            if (!_img)
            {
                Debug.LogError("イメージコンポーネントが取得できてないよ");
                return;
            }
        }
        _img.enabled = false;
        _img.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// フェード開始
    /// </summary>
    /// <param name="name">切り替えるシーン</param>
    public void FadeStrt(string name)
    {
        StartCoroutine(FadeOutAndLoadScene(name));
    }

    /// <summary>
    /// フェード完了したらシーンを切り替える
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator FadeOutAndLoadScene(string name)
    {
        _img.enabled = true;
        float elapsedTime = 0;
        Color startColor = _img.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        // フェードアウトアニメーションを実行
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _fadeDuration);
            _img.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        _img.color = endColor;
        SceneManager.LoadScene(name);
    }
}
