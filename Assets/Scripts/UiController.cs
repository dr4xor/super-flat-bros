using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    // Store a reference to the UI_DamageContainer prefab
    [SerializeField] private GameObject uiDamageContainerPrefab;
    
    // Store a reference to the UI_DamageContainer parent
    [SerializeField] private Transform uiDamageContainerParent;
    
    Dictionary<PlayerController, UI_DamageContainer> uiDamageContainers = new Dictionary<PlayerController, UI_DamageContainer>();
    
    void Start()
    {
        // Subscribe to the OnPlayerJoined and OnPlayerLeft events
        GameManager.Instance.OnPlayerJoined += OnPlayerJoined;
        GameManager.Instance.OnPlayerLeft += OnPlayerLeft;
    }


    void OnPlayerJoined(PlayerController playerController)
    {
        // Instantiate a UI_DamageContainer for the player
        // Set the playerController property of the UI_DamageContainer
        var damageContainer = Instantiate(uiDamageContainerPrefab, uiDamageContainerParent)
            .GetComponent<UI_DamageContainer>();
        damageContainer.playerController = playerController;
        uiDamageContainers.Add(playerController, damageContainer);
    }
    
    void OnPlayerLeft(PlayerController playerController)
    {
        // Destroy the UI_DamageContainer for the player
        Destroy(uiDamageContainers[playerController].gameObject);
        uiDamageContainers.Remove(playerController);
    }
}
