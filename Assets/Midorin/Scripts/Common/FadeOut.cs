using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// �t�F�[�h�A�E�g���ăV�[����؂�ւ���
/// </summary>
public class FadeOut : MonoBehaviour
{
    [SerializeField, Header("�t�F�[�h�I�u�W�F�N�g")]
    private Image img;

    [SerializeField, Header("�t�F�[�h�ɂ����鎞��")]
    private float fadeDuration = 1.0f;

    /// <summary>
    /// �X�^�[�g�C�x���g
    /// </summary>
    public void Start()
    {
        // Null�`�F�b�N
        if (!img)
        {
            img = GetComponent<Image>();
            if (!img)
            {
                Debug.LogError("�C���[�W�R���|�[�l���g���擾�ł��ĂȂ���");
                return;
            }
        }
        img.enabled = false;
        img.color = new Color(0, 0, 0, 0);
    }

    /// <summary>
    /// �t�F�[�h�J�n
    /// </summary>
    /// <param name="name">�؂�ւ���V�[��</param>
    public void FadeStrt(string name)
    {
        StartCoroutine(FadeOutAndLoadScene(name));
    }

    /// <summary>
    /// �t�F�[�h����������V�[����؂�ւ���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IEnumerator FadeOutAndLoadScene(string name)
    {
        img.enabled = true;
        float elapsedTime = 0;
        Color startColor = img.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f);

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            img.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        img.color = endColor;
        SceneManager.LoadScene(name);
    }
}
