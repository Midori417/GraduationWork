using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GundamBazookaEffControl : MonoBehaviour
{
    [SerializeField]
    GameObject pfb_eff_smoke;

    [SerializeField, Header("�������x")]
    float interval;

    [SerializeField, Header("�폜���x")]
    float destoryTime;

    // Start is called before the first frame update
    void Start()
    {
        Spawner();
    }

    void Spawner()
    {
        var obj = Instantiate(pfb_eff_smoke, transform.position, transform.rotation);
        Destroy(obj, destoryTime);
        Invoke("Spawner", interval);
    }
}
