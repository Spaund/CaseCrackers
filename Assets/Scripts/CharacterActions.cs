using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{

    public float speed;
    public Camera camera;
    private Vector2 direction;
    private Vector2 mousePos;
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
        Vector2 lookDir = mousePos - rb.position;
        rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }

    public void Move(InputAction.CallbackContext input)
    {
        direction = input.ReadValue<Vector2>().normalized;
    }

    public void Mouse(InputAction.CallbackContext input)
    {
        mousePos = camera.ScreenToWorldPoint(input.ReadValue<Vector2>());
    }
}
