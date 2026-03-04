using System;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    [SerializeField] Collider _collider;

    private void OnCollisionEnter(Collision collision)
    {
        if (UIManager.isGamePlaying)
            print("bruh");
    }
}
