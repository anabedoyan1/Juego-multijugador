using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    public static PlayerManager LocalPlayerInstance = null;
    public bool alive = true;
    public int playerNumber, health = 1;
    public GameObject myTarget;
    Vector3 myPosition, myRotation;
    [HideInInspector] public string myName;
    [SerializeField] TextMeshProUGUI nameText;
    Color textColor = new Color(0, 0.8f, 0.2f);
                
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        playerNumber = (byte)instantiationData[0];
        myName = (string)instantiationData[1];
        nameText.text = myName;
        if (photonView.IsMine)
        {
            nameText.color = textColor;
        }
        GameController.Instance.playersInGame.Add(this);
    }
    
    public void UpdateHealth(int _damage)
    {        
        health -= _damage;
        if (health <= 0)
        {
            photonView.RPC("Dead", RpcTarget.All);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.health);
            stream.SendNext(this.alive);
        }
        else
        {
            this.health = (int)stream.ReceiveNext();
            this.alive = (bool)stream.ReceiveNext();            
        }        
    }

    [PunRPC]
    public void Dead()
    {        
        Debug.Log("Parte 1: Dead");
        GameController.Instance.PlayerDeath(this);
    }    

}
