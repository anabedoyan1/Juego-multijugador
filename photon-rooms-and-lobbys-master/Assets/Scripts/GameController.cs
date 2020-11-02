using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    public static GameController Instance = null;
    [SerializeField] private GameObject tankGameObject;
    [SerializeField] GameObject[] spawnPositions = new GameObject[4];
    GameObject playerRefPosition, playerRotation;
    

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

    }

    public void Start()
    {
        if(PlayerManager.LocalPlayerInstance == null)
        {
            AssignPosition();
            object[] myCustomInitData = new object[]
            {
                (byte)PhotonNetwork.LocalPlayer.CustomProperties["nmr"],
                PhotonNetwork.NickName
            };
            PhotonNetwork.Instantiate("tank", AssignPosition().transform.position, AssignPosition().transform.rotation, 0, myCustomInitData);
        }
    }

    public GameObject AssignPosition()
    {
        playerRefPosition = spawnPositions[(byte)PhotonNetwork.LocalPlayer.CustomProperties["nmr"] - 1];
        return playerRefPosition;
    }

    public void PlayerDeath(PlayerManager _playerManager)
    {
        _playerManager.gameObject.SetActive(false);
    }
}
