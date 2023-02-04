using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Make GameController Singleton
    public static GameManager Instance;
    
    [SerializeField] private GameObject playerPrefab;
    
    public event Action<PlayerController> OnPlayerJoined = delegate { };
    public event Action<PlayerController> OnPlayerLeft = delegate { };
    
    List<PlayerController> players = new List<PlayerController>();

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
        InstantiatePlayer(isLocalPlayer: true);
        InstantiatePlayer();
    }
    
    
    void InstantiatePlayer(bool isLocalPlayer = false)
    { 
        PlayerController player = (Instantiate(playerPrefab) as GameObject).GetComponent<PlayerController>();
        
        player.PlayerName = "P" + (players.Count + 1); 
        player.IsLocalPlayer = isLocalPlayer;
        players.Add(player);
        
        OnPlayerJoined.Invoke(player);
    }
    
    public void RemovePlayer(PlayerController playerController)
    {
        players.Remove(playerController);
        OnPlayerLeft?.Invoke(playerController);
        Destroy(playerController.gameObject);
    }
    
}
