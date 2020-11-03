using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class GameController : MonoBehaviour
{
    public static GameController Instance = null;
    [SerializeField] private GameObject tankGameObject;
    [SerializeField] GameObject[] spawnPositions = new GameObject[4];
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI gameOverText;
    GameObject playerRefPosition, playerRotation;
    int remainingPlayers = 0;
    public List<PlayerManager> playersInGame = new List<PlayerManager>();

    const byte GameOverEvent = 1;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        gameOverPanel.SetActive(false);
    }

    public void Start()
    {
        if (PlayerManager.LocalPlayerInstance == null)
        {
            AssignPosition();
            object[] myCustomInitData = new object[]
            {
                (byte)PhotonNetwork.LocalPlayer.CustomProperties["nmr"],
                PhotonNetwork.NickName
            };
            PhotonNetwork.Instantiate("tank", AssignPosition().transform.position, AssignPosition().transform.rotation, 0, myCustomInitData);
        }
        remainingPlayers = PhotonNetwork.PlayerList.Length;
    }

    public GameObject AssignPosition()
    {
        playerRefPosition = spawnPositions[(byte)PhotonNetwork.LocalPlayer.CustomProperties["nmr"] - 1];
        return playerRefPosition;
    }

    public void PlayerDeath(PlayerManager _playerManager)
    {
        Debug.Log("Parte 2: GameController");
        _playerManager.alive = false;
        _playerManager.gameObject.SetActive(false);
        CheckAlivePlayers(_playerManager);
    }

    public void CheckAlivePlayers(PlayerManager _playerDead)
    {
        foreach (var player in playersInGame)
        {
            if (!player.alive)
            {
                remainingPlayers--;
                Debug.Log(remainingPlayers);
                if (remainingPlayers < 2)
                {
                    //gameOverPanel.SetActive(true);
                    if (!player.alive)
                        GameOver(false, player);
                    else
                        GameOver(true, player);
                }
            }
        }
    }
    public void GameOver(bool state, PlayerManager _player)
    {
        object[] content = new object[] { state, (byte)_player.playerNumber };
        RaiseEventOptions REO = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent(GameOverEvent,content, REO, SendOptions.SendReliable);
        // D:
    }    
}
 