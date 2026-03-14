using System;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    private UIManager _uiManager;

    private void OnEnable()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
    }

    private void OnCollisionEnter()
    {
        if (UIManager.isGamePlaying)
        {
            Time.timeScale = 0;
            _uiManager.LevelComplete();
        }
            
    }
}
