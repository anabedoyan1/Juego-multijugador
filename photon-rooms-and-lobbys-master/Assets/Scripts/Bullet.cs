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

        //if (collision.CompareTag("Target"))
        //{

        //}

        //if(collision.gameObject.GetComponent<PlayerManager>() != null)
        //{
        //    GameObject playerHit = collision.gameObject;
        //    playerHit.GetComponentInParent<PlayerManager>().photonView.RPC("Dead", RpcTarget.All);

        //    //if(collision.gameObject.GetComponent<PhotonView>().IsMine)
        //    //collision.gameObject.GetComponent<PlayerManager>().UpdateHealth(damage);
        //}

        //if (collision.gameObject.GetComponent<IDestructable>() != null)
        //{
        //    Debug.Log(collision.gameObject.GetComponentInParent<PhotonView>().Owner.NickName);
        //    PhotonView PVHit = collision.gameObject.GetComponentInParent<PhotonView>();
        //    PVHit.RPC("Dead", RpcTarget.All);
        //    //PlayerManager player = PVHit.gameObject.GetComponent<PlayerManager>();
        //    //Debug.Log(player);
        //    //collision.gameObject.GetComponent<IDestructable>().ReceiveDamage(damage);            
        //}
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
