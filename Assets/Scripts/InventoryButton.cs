using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public int ButtonIndex;
    [HideInInspector] public UIManager uiManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect()
    {
        print("버튼누름!");
    }
}
