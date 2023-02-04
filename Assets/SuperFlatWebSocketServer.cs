using UnityEngine;
using WebSocketServer;

public class SuperFlatWebSocketServer: WebSocketServer.WebSocketServer {

    public override void OnOpen(WebSocketConnection connection) {
        // Here, (string)connection.id gives you a unique ID to identify the client.
        Debug.Log(connection.id);
    }
  
    public override void OnMessage(WebSocketMessage message) {
        // (WebSocketConnection)message.connection gives you the connection that send the message.
        // (string)message.id gives you a unique ID for the message.
        // (string)message.data gives you the message content.
        Debug.Log(message.connection.id);
        Debug.Log(message.id);
        Debug.Log(message.data);
    }
  
    public override void OnClose(WebSocketConnection connection) {
        // Here is the same as OnOpen
        Debug.Log(connection.id);
    }

}