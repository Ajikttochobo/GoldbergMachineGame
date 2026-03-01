using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    [SerializeField] DeployManager deployManager;
    
    [Header("InventorySettings")]
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject inventoryButtonPrefab;
    
    [Header("GamePlayButtons")]
    [SerializeField] GameObject playPauseButton;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] GameObject resetButton;
    
    public int? activeInventoryButtonIndex = null;
    
    public static bool isGamePlaying = false;
    
    private InventoryButton[] inventoryButtons;
    
    [HideInInspector] public UnityEvent OnPlayPauseButtonPressed;
    [HideInInspector] public UnityEvent OnResetButtonPressed;
    private Image playPauseButtonImage;
    
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
        playPauseButtonImage = playPauseButton.GetComponent<Image>();
    }

    private void OnEnable()
    {
        OnPlayPauseButtonPressed.AddListener(PlayPauseButtonFunc);
        OnResetButtonPressed.AddListener(ResetButtonFunc);
    }

    public void OnInventoryButtonPressed(int buttonIndex)
    {
        if(buttonIndex == activeInventoryButtonIndex) //버튼 눌렀던거 한번 더 눌러서 선택 해제됨
        {
            EventSystem.current.SetSelectedGameObject(null);
            activeInventoryButtonIndex = null;
        }
        else //다른 버튼 눌러서 그 버튼 선택
        {
            EventSystem.current.SetSelectedGameObject(inventoryButtons[buttonIndex].gameObject);
            activeInventoryButtonIndex = buttonIndex;
        }
    }

    public void PlayPauseButtonPressed()
    {
        isGamePlaying = !isGamePlaying;
        OnPlayPauseButtonPressed?.Invoke();
    }

    public void ResetButtonPressed()
    {
        OnResetButtonPressed?.Invoke();
    }
    
    private void PlayPauseButtonFunc()
    {
        print("Play Pause Button");
        if (playPauseButtonImage.sprite == playSprite)
        {
            //play 상태로 바꿔야 함
            playPauseButtonImage.sprite = pauseSprite;
            
            EventSystem.current.SetSelectedGameObject(null);
            resetButton.SetActive(false);
            InventoryPanel.SetActive(false);
        }
        else
        {
            //pause 상태로 바꿔야 함
            playPauseButtonImage.sprite = playSprite;
            
            resetButton.SetActive(true);
            InventoryPanel.SetActive(true);

        }
    }

    private void ResetButtonFunc()
    {
        print("Reset Button");
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) //TODO 아니 또 왜 플레이모드 끝나면 버튼 선택되어 있는건데
        {
            EventSystem.current.SetSelectedGameObject(activeInventoryButtonIndex.HasValue && !isGamePlaying ? inventoryButtons[activeInventoryButtonIndex.Value].gameObject : null);
        }
        
        print(isGamePlaying);
    }
}
