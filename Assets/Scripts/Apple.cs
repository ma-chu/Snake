using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    // Параметр:
    public float respawnTime;                                            // Время перерождения
    
    [SerializeField] private Sprite apple; 
    [SerializeField] private Sprite stub;

    
    private void OnEnable()
    {
        LoadSave.Save += OnSave;
        LoadSave.Load += OnLoad;
    }
    private void OnDisable()
    {
        LoadSave.Save -= OnSave;
        LoadSave.Load -= OnLoad;
    }
    private void OnSave()
    {
        LoadSave.SaveSnapshot.respawnTime = respawnTime;
    }
    private void OnLoad()
    {
        respawnTime = LoadSave.SaveSnapshot.respawnTime;
    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = stub;
        GetComponent<Animator>().enabled = false;
        GetComponent<ParticleSystem>().Play();
        Invoke("Respawn", respawnTime);
    }
    
    private void Respawn()
    {
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = apple;
        GetComponent<Animator>().enabled = true;
    }
}
