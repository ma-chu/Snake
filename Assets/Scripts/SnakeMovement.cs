using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    // Параметры:
    [SerializeField] private float speed;                                            // Скорость движения змеи
    [SerializeField] private float accuracy;                                         // Точность хода змеи (раз в сколько сек перестраивать путь)

    
    [SerializeField] private Transform segmentPrefab;
    
    [SerializeField] private List<Transform> segments;
    [SerializeField] private List<Vector3> track;
    
    private float _currentSpeed;
    
    private void OnEnable()
    {
        GameManager.StartMove += OnStartMove;
        SnakeColliderHandler.ApplePicked += OnApplePicked;
    }

    private void OnDisable()
    {
        GameManager.StartMove -= OnStartMove;
        SnakeColliderHandler.ApplePicked -= OnApplePicked;
    }

    private void Start()
    {
        // Голова
        segments.Add(transform);                                                            
        track.Insert(0, transform.position);
    }

    private void FixedUpdate()
    {
        // Слежение головы за мышью
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);          // Input.mousePosition.z=0
        Vector3 snakeToMouse = mousePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, snakeToMouse); // какой именно вектор и куда подобрал перебором 

        // Точное перемещение сегментов тела
        foreach (var segment in segments)
        {
            Vector3 rotationInV3 =  segment.rotation * Vector3.up;
            segment.position += rotationInV3.normalized * _currentSpeed * Time.deltaTime; 
        }
    }

    private void OnStartMove()    
    {
        _currentSpeed = speed;
        InvokeRepeating("SegmentsMovement", 1f,accuracy);
    }
    
    private void OnApplePicked()    
    {
        var segment = Instantiate(segmentPrefab, track[1], transform.rotation.normalized);
        segments.Add(segment);
        
        if (segments.Count == 2)            // на втором сегменте коллайдер плохо
        {
            Destroy(segment.GetComponent<SphereCollider>());
        }
    }

    private void SegmentsMovement()   
    {
        // Обновляем трэк
        track.Insert(0, transform.position);
        if (track.Count > segments.Count + 1)
        {
            track.RemoveAt(segments.Count + 1);
        }

        // Обрабатываем сегменты
        for (int i = 1; i < segments.Count; i++)
        {
            // подровнять позиции
            segments[i].position = track[i];
            
            // вычислить вращение
            Vector3 rotationVector = track[i-1] - track[i];
            segments[i].rotation = Quaternion.LookRotation(Vector3.forward, rotationVector );
        }
    }
}
