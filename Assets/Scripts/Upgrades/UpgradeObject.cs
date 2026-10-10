using UnityEngine;

public abstract class UpgradeObject : ScriptableObject
{
    [Header("Display")]
    public string upgradeName;
    [TextArea] public string description;
    public Sprite upgradeIcon;
}
