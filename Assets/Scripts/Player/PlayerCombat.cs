using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Basic Attack Settings")]
    [SerializeField] float basicAttackDamage;
    [SerializeField] float basicAttackKnockbackForce;
    [SerializeField] Vector2 basicAttackOffset;
    [SerializeField] Vector2 basicAttackHitBox;

    [Header("Eat Attack Settings")]
    [SerializeField] BoxCollider2D eatCollider;
    [SerializeField] SpriteRenderer eatVisual;
    [SerializeField] Image eatCooldownUI;
    [SerializeField] private float startingLength = 1f;
    [SerializeField] private float maximumLength = 5f;
    [SerializeField] private float attackWidth = 1.5f;
    [SerializeField] private float chargeDuration = 2f;
    [SerializeField] private Color eatColor;
    private float chargeTimeElapsed;
    private bool eatCooldown;
    private Coroutine eatCooldownCoroutine;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] LayerMask damageableLayerMask;

    // Events
    public event System.Action OnEat;

    // Inputs
    InputAction basicAttackInput;
    InputAction eatInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basicAttackInput = InputSystem.actions.FindAction("BasicAttack");
        eatInput = InputSystem.actions.FindAction("Eat");

        eatVisual.color = eatColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (basicAttackInput.WasPressedThisFrame())
        {
            BasicAttack();
        }

        if (eatInput.IsPressed() && !eatCooldown)
        {
            eatCollider.enabled = true;
            eatVisual.enabled = true;

            chargeTimeElapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(chargeTimeElapsed / chargeDuration);

            float length = Mathf.Lerp(startingLength, maximumLength, progress);

            // Extend forwards along local Y
            eatCollider.size = new Vector2(attackWidth, length);

            // Matches the visual to the collider.
            // NOTE: SpriteRenderer needs a sliced sprite
            // for size to control dimensions reliably.
            // Ensure sprite is sliced.
            eatVisual.size = new Vector2(attackWidth, length);

            // Retain eat visual offset so that it extrudes outwards with the collider.
            eatVisual.gameObject.transform.localPosition = new Vector2(0f, length / 2f);
        }
        // If eat input is released, call eat event to execute actual eat logic
        else if (eatInput.WasReleasedThisFrame() && !eatCooldown)
        {
            // Invoke eat event (collider is listening)
            OnEat?.Invoke();

            // Propel the player forward while eating
            Movement movement = gameObject.GetComponent<Movement>();
            movement.TakeKnockback(transform.up, 50f * Mathf.Clamp01(chargeTimeElapsed / chargeDuration), 7f, 0.4f);

            // Start eat cooldown
            if (eatCooldownCoroutine != null)
            {
                StopCoroutine(eatCooldownCoroutine);
            }
            eatCooldownCoroutine = StartCoroutine(EatCooldown());

            chargeTimeElapsed = 0f;
            eatCollider.enabled = false;
            eatVisual.enabled = false;
        }
        else
        {
            chargeTimeElapsed = 0f;
            eatCollider.enabled = false;
            eatVisual.enabled = false;
        }
    }

    void BasicAttack()
    {
        // Play animation
        weaponAnimator.SetTrigger("BasicAttack");

        Vector2 attackPosition = (Vector2)transform.position + (Vector2)(transform.rotation * basicAttackOffset);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackPosition, basicAttackHitBox, transform.eulerAngles.z, damageableLayerMask);
        
        foreach (Collider2D collider in hitColliders)
        {
            // Applies damage
            if (collider.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.TakeDamage(basicAttackDamage);
            }

            // Applies knockback
            if (collider.TryGetComponent<Movement>(out Movement movement))
            {
                Vector2 knockbackDirection = collider.bounds.center - transform.position;
                knockbackDirection = knockbackDirection.normalized;

                movement.TakeKnockback(knockbackDirection, basicAttackKnockbackForce, 15f, 0.25f);
            }
        }
    }

    private IEnumerator EatCooldown()
    {
        eatCooldown = true;

        float elapsed = 0f;
        eatCooldownUI.gameObject.SetActive(true);
        eatCooldownUI.fillAmount = 1f;

        while (elapsed < playerStats.EatCooldown)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / playerStats.EatCooldown;
            eatCooldownUI.fillAmount = 1f - progress;

            yield return null;
        }

        eatCooldownUI.gameObject.SetActive(false);
        eatCooldownUI.fillAmount = 0f;

        eatCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.TRS(
        transform.position,
        transform.rotation,
        Vector3.one
        );

        Gizmos.DrawWireCube(basicAttackOffset, basicAttackHitBox);

        Gizmos.matrix = Matrix4x4.identity;
    }
}
