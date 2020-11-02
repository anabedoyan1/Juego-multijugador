using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private int damage = 1;
    [HideInInspector] public GameObject tankOwner;

    public void Start()
    {
        Debug.Log("La bala fue instanceada");
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<IDestructable>() != null && collision.gameObject != tankOwner)
        {
            GameObject objectHit = collision.gameObject;
            objectHit.GetComponent<IDestructable>().ReceiveDamage(damage);
        }
    }        
}
