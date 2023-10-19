using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.01f; // Adjust the speed as needed
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from the arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Move the player
        rb.velocity = movement * moveSpeed;

        // You can also limit the player's speed if needed
        // rb.velocity = movement.normalized * Mathf.Min(movement.magnitude * moveSpeed, moveSpeed);
    }
}