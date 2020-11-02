using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDestructable
{
    [SerializeField] GameObject myPlayer;
    public void ReceiveDamage(int _damage)
    {
        if (gameObject.GetComponentInParent<PhotonView>().IsMine)
        {
            myPlayer.GetComponent<PlayerManager>().UpdateHealth(_damage);
        }
        
    }
}
