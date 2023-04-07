using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterActions : MonoBehaviour
{
    [Header("Core components")]
    [SerializeField, SerializeAs("RigidBody")] private Rigidbody2D rb;

    void Update()
    {
        // Update movement position
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
        Flip();
    }
    
    // Move --------------------------------------------------------------
    [Header("Move")] 
    [SerializeField, Range(0f, 20f)] private float speed = 6f;

    private float direction;
    private bool isFacingRight = true;
    public void Move(InputAction.CallbackContext action)
    {
        direction = action.ReadValue<Vector2>().x;
    }
    
    private void Flip()
    {
        if (isFacingRight && direction < 0f || !isFacingRight && direction > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale; 
        }
    }
    
    // Jump --------------------------------------------------------------
    [Header("Jump")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Range(0f, 0.5f)] private float groundDistance = 0.2f;
    [SerializeField, Range(0f, 50f)] private float jumpPower = 16f;
    [SerializeField, Range(1f, 0f)] private float jumpDamp = 0.5f;
    
    public void Jump(InputAction.CallbackContext action)
    {
        if (action.performed && IsGrounded(groundDistance))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        if (action.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpDamp);
        }
    }
    
    private bool IsGrounded(float distance)
    {
        return Physics2D.OverlapCircle(groundCheck.position, distance, groundLayer);
    }
    
    // Abilities ---------------------------------------------------------
    [Header("Player ability")]
    [SerializeField] private Ability ability;
    
    public void Ability(InputAction.CallbackContext action)
    {
        if (action.performed) ability.perform();
        if (action.canceled) ability.cancel();
    }
}
