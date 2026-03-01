using System;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    private Vector3 originalPos;
    private Quaternion originalRot;
    
    private UIManager uiManager;

    private Rigidbody objRigidbody;

    private void OnEnable()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        
        objRigidbody = GetComponent<Rigidbody>();
        uiManager = FindAnyObjectByType<UIManager>();
        uiManager.OnPlayPauseButtonPressed.AddListener(playButtonPressed);
    }
    
    private void playButtonPressed()
    {
        if(UIManager.isGamePlaying)
            EnterPlayMode();
        else
            ExitPlayMode();
    }

    private void EnterPlayMode()
    {
        objRigidbody.isKinematic = false;
    }

    private void ExitPlayMode()
    {
        objRigidbody.isKinematic = true;
        objRigidbody.linearVelocity = Vector3.zero;
        
        transform.position = originalPos;
        transform.rotation = originalRot;
    }
}
