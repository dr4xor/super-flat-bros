using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NativeWebSocket;
using WebSocketServer;

public class WebSocketConnector : MonoBehaviour
{
    [SerializeField] private String websocketUrl = "wss://ggj.pockethost.app:42069";

    WebSocket websocket;

    private List<String> knownPlayerIds = new List<string>();

    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket(websocketUrl);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            var messageParts = message.Split(":");

            var playerId = messageParts[0];
            
            if (!knownPlayerIds.Contains(playerId))
            {
                knownPlayerIds.Add(playerId);
                GameManager.Instance.InstantiatePlayer(playerId);
            }
            
            Debug.Log(messageParts[1]);
            GameManager.Instance.OnWebsocketMessage(playerId, messageParts[1]);

            
            if (messageParts[1] == "DISCONNECTED")
            {
                knownPlayerIds.Remove(playerId);
                GameManager.Instance.RemovePlayer(playerId);
            }
            
        };
        
        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}