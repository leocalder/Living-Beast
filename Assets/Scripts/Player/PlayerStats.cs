using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float AttackDamage { get; private set; } = 10f;
    public float MaxHealth { get; private set; } = 5f;
    public float MoveSpeed { get; private set; } = 5f;
    public float EatSize { get; private set; } = 1.5f;
    public float EatChargeSpeed { get; private set; } = 1f;
    public float EatCooldown { get; private set; } = 20f;

}
