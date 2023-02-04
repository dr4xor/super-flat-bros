using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Make GameController Singleton
    public static GameController Instance;
    
    [SerializeField] private GameObject playerPrefab;
    
    List<PlayerController> players = new List<PlayerController>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void InstantiatePlayer()
    { 
        players.Add((Instantiate(playerPrefab) as GameObject).GetComponent<PlayerController>());
    }
    
}
