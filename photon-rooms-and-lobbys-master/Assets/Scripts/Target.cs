using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDestructable
{
    public void ReceiveDamage(int _health)
    {
        PlayerManager player = gameObject.GetComponentInParent<PlayerManager>();
        player.UpdateHealth(_health);
    }
}
