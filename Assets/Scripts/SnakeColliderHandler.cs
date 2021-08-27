using System;
using UnityEngine;

public class SnakeColliderHandler : MonoBehaviour
{
    public static event Action GameOver;
    public static event Action ApplePicked;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameOver?.Invoke();
        }
        
        if (other.CompareTag("Apple"))
        {
            ApplePicked?.Invoke();
        }
    }
}
