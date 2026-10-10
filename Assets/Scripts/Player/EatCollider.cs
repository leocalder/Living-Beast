using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EatCollider : MonoBehaviour
{
    [SerializeField] PlayerCombat playerCombat;

    private List<Collider2D> collidersInside = new List<Collider2D>();

    private void OnEnable()
    {
        playerCombat.OnEat += Eat;
    }

    private void OnDisable()
    {
        playerCombat.OnEat -= Eat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidersInside.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersInside.Remove(collision);
    }

    private void Eat()
    {
        if (collidersInside.Count > 0)
        {
            foreach (Collider2D collision in new List<Collider2D>(collidersInside))
            {
                // Eat corpses
                if (collision.gameObject.TryGetComponent<Corpse>(out Corpse corpse))
                {
                    corpse.GetEaten();
                }

                // Damage living enemies
                if (collision.gameObject.TryGetComponent<Damageable>(out Damageable damageable))
                {
                    damageable.TakeDamage(30f);

                    // Applies knockback
                    if (collision.gameObject.TryGetComponent<Movement>(out Movement movement))
                    {
                        Vector2 knockbackDirection = collision.transform.position - playerCombat.gameObject.GetComponent<Collider2D>().bounds.center;
                        knockbackDirection = knockbackDirection.normalized;

                        movement.TakeKnockback(knockbackDirection, 70f, 10f, 0.4f);
                    }
                }
            }
        }
    }
}
