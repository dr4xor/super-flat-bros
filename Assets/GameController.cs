using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    void InstantiatePlayer()
    {
        (Instantiate(playerPrefab) as GameObject).GetComponent<PlayerController>();
    }
    
}
