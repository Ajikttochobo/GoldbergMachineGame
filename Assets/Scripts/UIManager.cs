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
    [SerializeField] GameManager gameManager;
    
    [Header("InventorySettings")]
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject inventoryButtonPrefab;
    
    [Header("GamePlayButtons")]
    [SerializeField] GameObject playPauseButton;
    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] private GameObject resetButton;

    [Header("")]
    [SerializeField] GameObject levelCompletePanel;
    [SerializeField] private GameObject[] hideOnPlayMode;
    
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
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = deployManager.deployObjects[i].name;
            button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = deployManager.deployObjects[i].count.ToString();
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

    public void BackToMenuButtonPressed()
    {
        gameManager.MainMenu();
    }

    public void NextLevelButtonPressed()
    {
        gameManager.NextLevel();
    }
    
    private void PlayPauseButtonFunc()
    {
        if (playPauseButtonImage.sprite == playSprite)
        {
            //play 상태로 바꿔야 함
            playPauseButtonImage.sprite = pauseSprite;
            
            EventSystem.current.SetSelectedGameObject(null);
            activeInventoryButtonIndex = null;
            foreach (GameObject obj in hideOnPlayMode)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            //pause 상태로 바꿔야 함
            playPauseButtonImage.sprite = playSprite;
            
            foreach (GameObject obj in hideOnPlayMode)
            {
                obj.SetActive(true);
            }
        }
    }

    private void ResetButtonFunc()
    {
        
    }

    public void LevelComplete()
    {
        levelCompletePanel.SetActive(true);
    }
    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(activeInventoryButtonIndex.HasValue && !isGamePlaying ? inventoryButtons[activeInventoryButtonIndex.Value].gameObject : null);
        }
    }
}
