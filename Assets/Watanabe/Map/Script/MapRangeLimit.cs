using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �}�b�v�͈͐������I�u�W�F�N�g�𐶐�����
/// </summary>
public class MapRangeLimit : MonoBehaviour
{
    [SerializeField, Header("�}�b�v�T�C�Y")]
    private Vector2 mapSize = Vector2.one;

    [SerializeField, Header("�ǂ̐݌v�}")]
    private GameObject wall = null;

    /// <summary>
    /// �ŏ��Ɏ��s
    /// </summary>
    void Start()
    {
        Vector2 mapSizeHarf = mapSize / 2;
        {
            Vector2 pos = new Vector2(0, mapSizeHarf.y);
            Vector2 size = new Vector2(mapSize.x, 1);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(0, -mapSizeHarf.y);
            Vector2 size = new Vector2(mapSize.x, 1);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(mapSizeHarf.x, 0);
            Vector2 size = new Vector2(1, mapSize.y);
            CreateMapWall(pos, size);
        }
        {
            Vector2 pos = new Vector2(-mapSizeHarf.x, 0);
            Vector2 size = new Vector2(1, mapSize.y);
            CreateMapWall(pos, size);
        }

    }

    /// <summary>
    /// �ǂ��쐬
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    void CreateMapWall(Vector2 pos, Vector2 size)
    {
        if (!wall)
        {
            Debug.LogError("�ǂ̐݌v�}���o�^����Ă��Ȃ�");
            return;
        }
        GameObject obj = Instantiate(wall);
        obj.transform.position = new Vector3(pos.x, 0.5f, pos.y);
        obj.transform.localScale = new Vector3(size.x, 1, size.y);

        obj.transform.SetParent(transform);
    }
}
