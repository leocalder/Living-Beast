using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent (typeof(Health))]
public class Damageable : MonoBehaviour
{
    [SerializeField] GameObject bloodFloorDecal1;
    [SerializeField] GameObject bloodFloorDecal2;
    [SerializeField] GameObject bloodFloorDecal3;
    //[SerializeField] GameObject impactEffect;

    private Health health;
    private Coroutine hitFlashCoroutine;
    private GameObject player;
    private Color originalColor;

    private void Awake()
    {
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(float damage)
    {
        health.SubtractHealth(damage);

        // Generates floor decal
        // 50% chance of executing
        if (Random.Range(0,2) == 0)
        {
            GenerateFloorDecal();
        }

        // Hit impact particle effect
        //Instantiate(impactEffect, gameObject.GetComponent<Collider2D>().ClosestPoint(player.transform.position), impactEffect.transform.rotation, transform);

        // Hit flash effect (coroutine)
        HitFlash();
    }

    private void GenerateFloorDecal()
    {
        // Projects blood decal on floor
        int rand = Random.Range(1, 4);
        GameObject randomDecal;
        if (rand == 1)
        {
            randomDecal = bloodFloorDecal1;
        }
        else if (rand == 2)
        {
            randomDecal = bloodFloorDecal2;
        }
        else if (rand == 3)
        {
            randomDecal = bloodFloorDecal3;
        }
        else
        {
            randomDecal = null;
        }
        GameObject blood = Instantiate(randomDecal, transform.position, Quaternion.identity);
        blood.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        float bloodScale = Random.Range((blood.transform.localScale.x - 0.15f), blood.transform.localScale.x - 0.1f);
        blood.transform.localScale = new Vector3(bloodScale, bloodScale, 0);
        Vector3 behind = -transform.up;
        float behindDistance = Random.Range(0.6f, 3f);
        Vector3 right = transform.right;
        float horizontalOffset = Random.Range(-0.3f, 0.3f);
        Vector2 bloodPosition = transform.position + behind * behindDistance + right * horizontalOffset;
        blood.transform.position = bloodPosition;
    }

    // Creates white hit effect upon impact
    public void HitFlash()
    {
        if (gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
        {
            if (hitFlashCoroutine != null)
            {
                StopCoroutine(hitFlashCoroutine);
                spriteRenderer.color = originalColor;
            }

            hitFlashCoroutine = StartCoroutine(HitFlashCoroutine(spriteRenderer));
        }
        else
        {
            Debug.LogError("This game object needs a sprite renderer attached so that the hit flash function can be viable.");
        }
    }

    private IEnumerator HitFlashCoroutine(SpriteRenderer spriteRenderer)
    {
        originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.08f);

        spriteRenderer.color = originalColor;
    }
}
