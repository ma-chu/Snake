using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime;                 //   ориентировочное время движения камеры в заданную позицию, вернее, пауза перед движением        

    [SerializeField] private Transform snakesHead;
    
    //private Camera _camera;
    private Vector3 _moveVelocity;                       // ос от метода Vector3.SmoothDamp - реальная скорость движения камеры          
    private Vector3 _desiredPosition;         


    private void Awake()
    {
        //_camera = GetComponent<Camera>();
    }

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
        LoadSave.SaveSnapshot.dampTime = dampTime;
    }
    private void OnLoad()
    {
        dampTime = LoadSave.SaveSnapshot.dampTime;
    }

    private void FixedUpdate()
    {
        _desiredPosition = new Vector3 (snakesHead.position.x, snakesHead.position.y, transform.position.z);
        
        Move();
    }


    private void Move()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _moveVelocity, dampTime);
    }
}