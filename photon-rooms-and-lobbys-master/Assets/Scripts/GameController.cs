using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    public static GameController Instance = null;
    [SerializeField] private GameObject tankGameObject;
    [SerializeField] GameObject[] spawnPositions = new GameObject[4];
    int playerNum;
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
        playerNum = RoomCtrl.Instance.LocalPlayerNum;
        Debug.Log(playerNum);
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
        playerRefPosition = spawnPositions[playerNum-1];
        return playerRefPosition;
    }

    public void PlayerDeath(PlayerManager _playerManager)
    {
        _playerManager.gameObject.SetActive(false);
    }
}
