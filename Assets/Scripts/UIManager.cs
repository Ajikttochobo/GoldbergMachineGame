using TMPro;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] DeployManager deployManager;
    
    [Header("InventorySettings")]
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject inventoryButtonPrefab;
    
    [Header("GamePlayButtons")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject resetButton;
    
    private InventoryButton[] inventoryButtons;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<InventoryButton> buttonList = new List<InventoryButton>();
        
        for (int i = 0; i < deployManager.deployObjects.Length; i++)
        {
            GameObject button = Instantiate(inventoryButtonPrefab, InventoryPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = deployManager.deployObjects[i].name;
            button.GetComponent<InventoryButton>().ButtonIndex = i;
            button.GetComponent<InventoryButton>().uiManager = this;
            buttonList.Add(button.GetComponent<InventoryButton>());
        }
        
        inventoryButtons = buttonList.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
