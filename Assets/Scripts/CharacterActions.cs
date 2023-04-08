using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterActions : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        // Automatically get Rigidbody
        // Rigidbody2D will be used in player movement and the script won't work without it
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Update calls
        // Create a function for each character input
        MoveUpdate();
        JumpUpdate();
    }
    
    // Move --------------------------------------------------------------
    [Header("Movement")] 
    [SerializeField, Range(1f, 80f)] private float movementAcceleration = 50f;
    [SerializeField, Range(1f, 25f)] private float maxMoveSpeed = 12f;
    [SerializeField, Range(0f, 50f)] private float groundLinearDrag = 10f;
    [SerializeField, Range(0f, 1f)] private float dragThreshold = 0.4f;

    private float _direction;
    private bool _facingRight = true;
    private bool ChangingDirection => (_rb.velocity.x > 0f && _direction < 0f) || (_rb.velocity.x < 0f && _direction > 0f);
    
    public void Move(InputAction.CallbackContext action)
    {
        // This gets the Vector2D input of the Unity Input System
        // Stores only x value as we only need the x value
        _direction = action.ReadValue<Vector2>().x;
    }

    private void MoveUpdate()
    {
        // Adds movement using forces
        _rb.AddForce(new Vector2(_direction * movementAcceleration, 0f));

        // Clamps velocity if higher than move speed
        if (Mathf.Abs(_rb.velocity.x) > maxMoveSpeed)
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * maxMoveSpeed, _rb.velocity.y);

        // Adds drag to decelerate the character
        if (IsGrounded())
            // If player is grounded, applies ground drag (declared in Move)
            _rb.drag = Mathf.Abs(_direction) < dragThreshold || ChangingDirection ? groundLinearDrag : 0f;
        else if (!IsGrounded())
            // If player is not grounded, applies air drag (declared in Jump)
            _rb.drag = airLinearDrag;
        
        // Flips the sprite to face the _direction the character is moving
        if (_facingRight && _direction < 0f || !_facingRight && _direction > 0f)
            Flip();
    }
    
    private void Flip()
    {
        _facingRight = !_facingRight;
        var t = transform;
        Vector3 localScale = t.localScale;
        localScale.x *= -1f;
        t.localScale = localScale;
    }
    
    // Jump --------------------------------------------------------------
    [Header("Jump")]
    [SerializeField, Range(0f, 50f)] private float jumpPower = 16f;
    [SerializeField, Range(0f, 6f)] private float airLinearDrag = 2.5f;
    [SerializeField, Range(1f, 10f)] private float fallMultiplier = 2.5f;
    [SerializeField, Range(1f, 0f)] private float smallJumpDamp = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(0f, 1.5f)] private float raycastLenght = 0.2f;
    [SerializeField] private bool drawGizmos;
    
    public void Jump(InputAction.CallbackContext action)
    {
        if (action.performed && IsGrounded())
        {
            _rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
        if (action.canceled && _rb.velocity.y > 0f)
        {
            // Makes the character make a small jump if the button is released quickly
            var vel = _rb.velocity;
            vel = new Vector2(vel.x, vel.y * smallJumpDamp);
            _rb.velocity = vel;
        }
    }

    private void JumpUpdate()
    {
        // Makes the character fall faster
        if (_rb.velocity.y < 0)
            _rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
    }
    
    private bool IsGrounded()
    {
        // Checks if the character is touching the ground
        return Physics2D.Raycast(transform.position * raycastLenght, Vector2.down, raycastLenght, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (drawGizmos)
            // Draw gizmos for Jump function
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght);
    }

    // Abilities ---------------------------------------------------------
    [Header("Player Ability")] 
    [SerializeField] private bool activateAbility;
    [SerializeField] private Ability ability;
    
    public void Ability(InputAction.CallbackContext action)
    {
        // Invoke function from Ability class component
        // Component needs to be added to player in order to work
        if (action.performed && activateAbility) ability.perform();
        if (action.canceled && activateAbility) ability.cancel();
    }
}
