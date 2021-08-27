using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    // Параметр:
    [SerializeField] private float respawnTime;                                            // Время перерождения
    
    [SerializeField] private Sprite apple; 
    [SerializeField] private Sprite stub;

    void OnTriggerEnter(Collider other)
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = stub;
        GetComponent<Animator>().enabled = false;
        Invoke("Respawn", respawnTime);
    }
    
    private void Respawn()
    {
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = apple;
        GetComponent<Animator>().enabled = true;
    }
}
