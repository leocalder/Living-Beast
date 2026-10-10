using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    [Header("Level-Up UI")]
    [SerializeField] GameObject levelUpMenu;
    [SerializeField] GameObject upgradeChoice1UI;
    [SerializeField] GameObject upgradeChoice2UI;
    [SerializeField] GameObject upgradeChoice3UI;

    [Header("Upgrade Pools")]
    [SerializeField] UpgradePoolObject statUpgradePool;

    [Header("References")]
    [SerializeField] private PlayerLevel playerLevel;

    private UpgradeObject upgradeChoice1;
    private UpgradeObject upgradeChoice2;
    private UpgradeObject upgradeChoice3;

    private void OnEnable()
    {
        playerLevel.OnLevelUp += LevelUp;
    }

    private void OnDisable()
    {
        playerLevel.OnLevelUp -= LevelUp;
    }

    private void LevelUp(int level)
    {
        levelUpMenu.SetActive(true);

        // Choose 3 random stat upgrades to offer the player
        var picked = statUpgradePool.upgrades.OrderBy(x => UnityEngine.Random.value).Take(3).ToList();
        upgradeChoice1 = picked[0];
        upgradeChoice2 = picked[1];
        upgradeChoice3 = picked[2];

        // Retrieve image component from upgrade choice buttons
        Image upgradeChoice1Image = upgradeChoice1UI.GetComponent<Image>();
        Image upgradeChoice2Image = upgradeChoice2UI.GetComponent<Image>();
        Image upgradeChoice3Image = upgradeChoice3UI.GetComponent<Image>();

        // Set color of upgrade choice buttons
        // ---
        // If upgrade is a stat upgrade...
        if (upgradeChoice1 is StatUpgradeObject statUpgrade1)
        {
            upgradeChoice1Image.color = GetUpgradeColor(statUpgrade1.statType);
        }
        if (upgradeChoice2 is StatUpgradeObject statUpgrade2)
        {
            upgradeChoice2Image.color = GetUpgradeColor(statUpgrade2.statType);
        }
        if (upgradeChoice3 is StatUpgradeObject statUpgrade3)
        {
            upgradeChoice3Image.color = GetUpgradeColor(statUpgrade3.statType);
        }

        // Set names of upgrade choice buttons
        upgradeChoice1UI.GetComponentInChildren<TextMeshProUGUI>().text = upgradeChoice1.name;
        upgradeChoice2UI.GetComponentInChildren<TextMeshProUGUI>().text= upgradeChoice2.name;
        upgradeChoice3UI.GetComponentInChildren<TextMeshProUGUI>().text = upgradeChoice3.name;

        // Set transparency of upgrade choice buttons
        Color upgradeChoice1Color = upgradeChoice1Image.color;
        upgradeChoice1Color.a = 0.5f;
        upgradeChoice1Image.color = upgradeChoice1Color;
        Color upgradeChoice2Color = upgradeChoice2Image.color;
        upgradeChoice2Color.a = 0.5f;
        upgradeChoice2Image.color = upgradeChoice2Color;
        Color upgradeChoice3Color = upgradeChoice3Image.color;
        upgradeChoice3Color.a = 0.5f;
        upgradeChoice3Image.color = upgradeChoice3Color;
    }
    private Color GetUpgradeColor(StatType type)
    {
        switch (type)
        {
            case StatType.AttackDamage: return Color.red;
            case StatType.MaxHealth: return Color.green;
            case StatType.MoveSpeed: return Color.cyan;
            case StatType.EatSize: return Color.purple;
            case StatType.EatChargeSpeed: return Color.deepPink;
            case StatType.EatCooldown: return Color.pink;

            default: return Color.white;
        }
    }
}
