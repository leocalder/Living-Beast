using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    Rigidbody2D rb2D;
    bool canMove = true;

    Coroutine knockbackCoroutine;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveVector, float moveSpeed)
    {
        if (canMove)
        {
            rb2D.linearVelocity = new Vector3(moveVector.x * moveSpeed, moveVector.y * moveSpeed, 0);
        }
    }

    public void EnableMove()
    {
        canMove = true;
    }

    public void DisableMove()
    {
        canMove = false;
    }

    public void ZeroVelocity()
    {
        rb2D.linearVelocity = Vector3.zero;
    }

    public void TakeKnockback(Vector2 knockbackDirection, float knockbackForce, float damping, float duration)
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }

        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(knockbackDirection, knockbackForce, damping, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDirection, float knockbackForce, float damping, float duration)
    {
        DisableMove();
        ZeroVelocity();
        float originalDamping = rb2D.linearDamping;
        rb2D.linearDamping = damping;
        rb2D.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        rb2D.linearDamping = originalDamping;
        ZeroVelocity();
        EnableMove();

        knockbackCoroutine = null;
    }
}
