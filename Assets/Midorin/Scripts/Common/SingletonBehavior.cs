using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シングルトンコンポーネント
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T I => Instance;

    public static T Instance
    {
        get
        {
            if (_Instance != null)
                return _Instance;
            _Instance = FindFirstObjectByType<T>();
            return _Instance;
        }
    }

    private static T _Instance = null;

    protected virtual void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this as T;
            return;
        }

        if (_Instance == this)
            return;

        Destroy(this);
    }
}
