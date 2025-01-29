using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �o�g�����C��UI�؂�ւ�
/// ����̓o�g���}�l�[�W���ɂ�����
/// </summary>
public class BattleEventUIControl : MonoBehaviour
{
    [SerializeField, Header("")]
    private Sprite stanby;

    [SerializeField, Header("")]
    private Sprite go;

    [SerializeField, Header("")]
    private Sprite win;

    [SerializeField, Header("")]
    private Sprite lose;

    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();

        img.enabled = false;
    }

    /// <summary>
    /// �摜�̕\���Ɛ؂�ւ�
    /// </summary>
    /// <param name="sprite"></param>
    private void ImgChange(Sprite sprite)
    {
        if (!sprite) return;
        img.enabled = true;
        img.sprite = sprite;
    }

    #region BattleManager�ő��삷��֐�
    /// <summary>
    /// �X�^���o�C
    /// </summary>
    public void Stanby()
    {
        ImgChange(stanby);
    }

    /// <summary>
    /// �S�[
    /// </summary>
    public void Go()
    {
        ImgChange(go);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Win()
    {
        ImgChange(win);
    }

    /// <summary>
    /// �s�k
    /// </summary>
    public void Lose()
    {
        ImgChange(lose);
    }

    /// <summary>
    /// �摜��\�����Ȃ�
    /// </summary>
    public void NoImg()
    {
        img.enabled = false;
    }
    #endregion
}
