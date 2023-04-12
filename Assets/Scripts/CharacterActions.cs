using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    // Serialized variables
    [SerializeField, Range(1f, 10f)] private float speed; // Unit: m/s
    [SerializeField, Range(0f, 1f)] private float movementAcceleration = 0.1f; // Unit: s
    [SerializeField, Range(0f, 0.6f)] private float cameraAcceleration = 0.05f; // Unit: s
    
    // GameObjects
    private Camera _camera;
    private Rigidbody2D _rb;
    
    private Vector2 _direction; 
    private Vector2 _mousePos; 
    private Vector2 _smoothDirection; 
    private Vector2 _smoothDirectionVelocity; 
    private Vector2 _smoothRotation; 
    private Vector2 _smoothRotationVelocity; 

    private void Awake()
    {
        // Gets Objects and Components automatically
        // Saves time when creating new scenes as you don't have to select the camera every time
        // Because physics aren't important, Rigidbody is also created here to avoid forgetting it
        _camera = FindObjectOfType<Camera>();
        _rb = gameObject.AddComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
    }
    
    void FixedUpdate()
    {
        /* MOVEMENT */   
        // Gradually changes direction over time adding acceleration and deceleration
        // The vector is smoothed by some spring-damper like function, which will never overshoot
        // SmoothDamp multiplies by Time.deltaTime by default, so _smoothDirection unit is m*s
        _smoothDirection = Vector2.SmoothDamp(
            current: _smoothDirection, 
            target: _direction, 
            currentVelocity: ref _smoothDirectionVelocity, 
            smoothTime: movementAcceleration);
        _rb.velocity = _smoothDirection * speed;
        
        /* CAMERA */
        // Gradually changes rotation over time making direction changes smoother
        // Uses same SmoothDamp function in MOVEMENT, but applied on an scalar rather than a vector
        Vector2 lookDir = _mousePos - _rb.position;
        _smoothRotation = Vector2.SmoothDamp(
            current: _smoothRotation, 
            target: lookDir, 
            currentVelocity: ref _smoothRotationVelocity, 
            smoothTime: cameraAcceleration);
        // Gets angle using trigonometry between position and (0, 1)
        // Player should be facing up on editor for rotation to work properly 
        _rb.rotation = Mathf.Atan2(_smoothRotation.y, _smoothRotation.x) * Mathf.Rad2Deg - 90f;
    }

    public void Move(InputAction.CallbackContext input)
    {
        // Gets Vector2D from PlayerInput Component (move)
        _direction = input.ReadValue<Vector2>().normalized;
    }

    public void Mouse(InputAction.CallbackContext input)
    {
        // Gets Vector2D from PlayerInput Component (look)
        // Camera is used to transform unit from screen pixels to meters
        _mousePos = _camera.ScreenToWorldPoint(input.ReadValue<Vector2>());
    }
}
