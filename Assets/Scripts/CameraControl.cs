using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float dampTime = 0.2f;       //   ориентировочное время движения камеры в заданную позицию, вернее, пауза перед движением        

    [SerializeField] private Transform snakesHead;
    
//    private Camera _camera;
    private Vector3 _moveVelocity;                       // ос от метода Vector3.SmoothDamp - реальная скорость движения камеры          
    private Vector3 _desiredPosition;         


 /*   private void Awake()
    {
        _camera = GetComponent<Camera>();    
    }
*/

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