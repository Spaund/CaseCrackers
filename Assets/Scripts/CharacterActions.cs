using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{

    public float speed;
    private Vector2 direction;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    public void Move(InputAction.CallbackContext input)
    {
        direction = input.ReadValue<Vector2>().normalized;
    }
}
