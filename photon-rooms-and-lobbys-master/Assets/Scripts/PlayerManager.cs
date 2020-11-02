using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
{
    public static PlayerManager LocalPlayerInstance = null;
    bool alive = true;
    int playerNumber, health = 1;
    public GameObject myTarget;
    Vector3 myPosition, myRotation;
    [HideInInspector] public string myName;
    [SerializeField] TextMeshProUGUI nameText;
    Color textColor = new Color(0, 0.8f, 0.2f);

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
        if (photonView.IsMine)
        {
            nameText.color = textColor;
        }
    }

    [PunRPC]
    public void UpdateHealth(int _health)
    {
        health -= _health;
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
        }
        else
        {
            this.health = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void Dead()
    {
        //gameObject.SetActive(false);
        GameController.Instance.PlayerDeath(this);
    }

}
