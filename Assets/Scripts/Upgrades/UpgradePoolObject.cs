using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Pool", menuName = "Upgrades/Upgrade Pool")]
public class UpgradePoolObject : ScriptableObject
{
    public List<UpgradeObject> upgrades;
}
