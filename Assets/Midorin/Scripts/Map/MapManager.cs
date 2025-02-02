using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField, Header("リスポーン位置")]
    private List<Transform> _responTrs;

    public List<Transform> responTrs => _responTrs;
}
