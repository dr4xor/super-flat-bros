using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using WebSocketServer;

public class GameManager : MonoBehaviour
{
    // Make GameController Singleton
    public static GameManager Instance;
    
    [SerializeField] private GameObject playerPrefab;
    
    public event Action<PlayerController> OnPlayerJoined = delegate { };
    public event Action<PlayerController> OnPlayerLeft = delegate { };
    
    // < websocket_id , PlayerController>
    Dictionary<string, PlayerController> players = new();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitDebugGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitDebugGame()
    {
        yield return new WaitForSeconds(2);
        print(playerPrefab.tag);
        InstantiatePlayer("debug1", isLocalPlayer: true);
        InstantiatePlayer("debug2", isLocalPlayer: true);
    }
    
    
    public void InstantiatePlayer(String playerId, bool isLocalPlayer = false)
    { 
        PlayerController player = (Instantiate(playerPrefab) as GameObject).GetComponent<PlayerController>();
        
        player.PlayerName = "P" + (players.Count + 1); 
        player.PlayerId = playerId;
        player.IsLocalPlayer = isLocalPlayer;
        players.Add(playerId, player);
        
        OnPlayerJoined.Invoke(player);
    }
    
    public void RemovePlayer(String playerId)
    {
        var playerController = players[playerId];
        players.Remove(playerId);

        OnPlayerLeft?.Invoke(playerController);
        Destroy(playerController.gameObject);
    }


    public void OnWebsocketOpened(WebSocketConnection connection)
    {
        InstantiatePlayer(connection.id);
    }
    
    public void OnWebsocketClosed(WebSocketConnection connection)
    {
        RemovePlayer(connection.id);
    }
    
    public void OnWebsocketMessage(WebSocketMessage message)
    {
        var playerController = players[message.connection.id];
        if (message.data == "ATTACK")
        {
            playerController.Attack();
            return;
        }
        
        if (message.data.StartsWith("MOVE"))
        {
            var x = message.data.Split('/')[1];
            var y = message.data.Split('/')[2];

            playerController.UpdateMovement(float.Parse(x, CultureInfo.InvariantCulture),-float.Parse(y, CultureInfo.InvariantCulture));
            return;
        }
    }
    
}
