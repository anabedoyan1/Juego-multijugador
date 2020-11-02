using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    public static PlayerManager LocalPlayerInstance = null;
    bool alive = true;
    public GameObject myTarget;    
    Vector3 myPosition, myRotation;
    float speed = 20;
    int playerNumber, health = 1;
    public string myName;
    [SerializeField]TextMeshProUGUI nameText;

    public void Awake()
    {
        if (LocalPlayerInstance == null)
            LocalPlayerInstance = this;
        else
            Destroy(this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        playerNumber = (byte)instantiationData[0];
        myName = (string)instantiationData[1];
        nameText.text = myName;
    }   

    [PunRPC]
    public void UpdateHealth(int _health)
    {
        Debug.Log("Ouch");
        if (photonView.IsMine)
        {
            health -= _health;
            if (health <= 0)
            {
                photonView.RPC("Die", RpcTarget.All);
            }
        }     
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    [PunRPC]
    public void Die()
    {
        GameController.Instance.PlayerDeath(this);
        Debug.Log("Die");
    }

}
