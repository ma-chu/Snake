using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    // GameManager
    public int width;
    public int height;
    public Vector3[] obstacles;
    public Vector3[] apples;
    // CameraControl
    public float dampTime;
    // SnakeMovement
    public float speed;  
    public float accuracy;
    // Apple (prefab)
    public float respawnTime;  
}
