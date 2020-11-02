using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private int damage = 1;

    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.GetComponent<IDestructable>() != null)
        {
            GameObject objectHit = collision.gameObject;
            objectHit.GetComponent<IDestructable>().ReceiveDamage(damage);
        }
        Destroy(gameObject);
    }
}
