using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [HideInInspector] public GameObject tankOwner;
    bool canDestroy = false;
    Collider m_collider;

    public void Start()
    {
        StartCoroutine(DestroyCounter());        
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<IDestructable>() != null)
        {            
            collision.gameObject.GetComponent<IDestructable>().ReceiveDamage(damage);            
        }
        
        if (collision.gameObject != null && canDestroy)
        {
            Destroy(gameObject);
        }
    } 
    
    public IEnumerator DestroyCounter()
    {
        yield return new WaitForSeconds(0.3f);
        if (!canDestroy)
        canDestroy = true;
    }
}
