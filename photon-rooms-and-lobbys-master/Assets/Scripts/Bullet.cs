using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public void Start()
    {
        Debug.Log("La bala fue instanciada");
    }
    public void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.GetComponent<IDestructable>() != null)
        {
            Debug.Log("Mueree");
            collision.gameObject.GetComponent<IDestructable>().ReceiveDamage(1);
        }
        Destroy(gameObject);
    }
}
