using UnityEngine;

[RequireComponent (typeof(Movement))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollowerBehaviour : MonoBehaviour
{
    Movement movement;
    GameObject player;
    Rigidbody2D rb2D;

    [Header("Enemy Settings")]
    [SerializeField] float moveSpeed;

    private void Awake()
    {
        movement = GetComponent<Movement> ();
        rb2D = GetComponent<Rigidbody2D> ();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        MoveTowardPlayer();
        RotateTowardPlayer();
    }

    private void MoveTowardPlayer()
    {
        Vector2 moveDirection = player.transform.position - transform.position;
        moveDirection = moveDirection.normalized;
        movement.Move(moveDirection, moveSpeed);
    }

    private void RotateTowardPlayer()
    {
        Vector2 directionTowardPlayer = player.transform.position - transform .position;
        float lookAngle = Mathf.Atan2(directionTowardPlayer.y, directionTowardPlayer.x) * Mathf.Rad2Deg - 90f;

        rb2D.MoveRotation(lookAngle);
    }
}
