using UnityEngine;

public enum StatType
{
    AttackDamage,
    MaxHealth,
    MoveSpeed,
    EatSize,
    EatChargeSpeed,
    EatCooldown,
}

[CreateAssetMenu(fileName = "New Stat Upgrade", menuName = "Upgrades/Stat Upgrade")]
public class StatUpgradeObject : UpgradeObject
{
    [Header("Stat Effect")]
    public StatType statType;
    public float amount;
}
