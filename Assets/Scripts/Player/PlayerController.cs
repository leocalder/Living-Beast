using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] float moveSpeed;

    Movement movement;
    Rigidbody2D rb2D;
    Vector2 moveInputVector;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        moveInputVector = InputSystem.actions.FindAction("Move").ReadValue<Vector2>(); // Stores move input

        UpdatePlayerRotation();
    }

    // Called every physics frame
    private void FixedUpdate()
    {
        movement.Move(moveInputVector, moveSpeed);
    }

    void UpdatePlayerRotation()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Camera cam = Camera.main;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawLine(transform.position, mouseWorldPos, Color.blue); // Draws line from player to mouse

        Vector2 lookDirection = mouseWorldPos - (Vector2)transform.position; // Find vector direction from player to mouse
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        rb2D.MoveRotation(lookAngle);
    }
}
