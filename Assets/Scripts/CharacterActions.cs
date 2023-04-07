using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterActions : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        // Automatically get Rigidbody
        // Rigidbody2D will be used in player movement and the script won't work without it
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Update calls
        // Create a function for each character input
        MoveUpdate();
    }
    
    // Move --------------------------------------------------------------
    [Header("Move")] 
    [SerializeField, Range(0f, 20f)] private float speed = 6f;
    [SerializeField] private AnimationCurve acceleration;
    [SerializeField, Range(0.1f, 10f)] private float accelSpeed;
    [SerializeField] private AnimationCurve decceleration;
    [SerializeField, Range(0.1f, 10f)] private float deccelSpeed;

    private float direction;
    private float accelTime;
    private float deccelTime;
    private bool isFacingRight = true;
    public void Move(InputAction.CallbackContext action)
    {
        // Set time variables of curves to create acceleration
        if (action.performed) accelTime = 0;
        if (action.canceled) deccelTime = 0;
        
        // Reads input value from the controller/keyboard
        direction = action.ReadValue<Vector2>().x;
    }

    private void MoveUpdate()
    {
        rb.velocity = new Vector2(
            x: direction * speed * acceleration.Evaluate(accelTime) + transform.localScale.x * speed * decceleration.Evaluate(deccelTime) * acceleration.Evaluate(accelTime), 
            y: rb.velocity.y);

        accelTime += Time.deltaTime * accelSpeed;
        deccelTime += Time.deltaTime * deccelSpeed;
        
        // Flips the sprite to face the direction the character is moving
        if (isFacingRight && direction < 0f || !isFacingRight && direction > 0f)
            Flip();
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        var t = transform;
        Vector3 localScale = t.localScale;
        localScale.x *= -1f;
        t.localScale = localScale;
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
        // Jump changes directly the vertical velocity of the character
        // It's not the most accurate way but it will work for now
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
        // Checks if the character is touching the ground
        return Physics2D.OverlapCircle(groundCheck.position, distance, groundLayer);
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
