using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_DamageContainer : MonoBehaviour
{
    public TextMeshProUGUI DamageTextField;
    public TextMeshProUGUI PlayerNameTextField;
    
    public PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerNameTextField.SetText(playerController.PlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        DamageTextField.SetText(Mathf.RoundToInt(playerController.DamageValue).ToString() + "%");
    }
}
