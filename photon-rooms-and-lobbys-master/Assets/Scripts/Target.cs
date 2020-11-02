using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDestructable
{
    public void ReceiveDamage(int _health)
    {
        Debug.Log("Daño");
        PlayerManager.LocalPlayerInstance.photonView.RPC("UpdateHealth", RpcTarget.All, _health);
    }
}
