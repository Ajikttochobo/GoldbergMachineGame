using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    public int ButtonIndex;
    [HideInInspector] public UIManager uiManager;

    public void OnSelect()
    {
        uiManager.OnInventoryButtonPressed(ButtonIndex);
    }
}
