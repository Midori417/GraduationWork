using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField, Header("—^‚¦‚éƒ_ƒ[ƒW")]
    private int damage;

    /// <summary>
    /// Õ“Ë‚µ‚½‚Æ‚«‚Éˆ—
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MS")
        {
            other.GetComponent<BaseMs>().Damage(damage);
        }

    }
}
