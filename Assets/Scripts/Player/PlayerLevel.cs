using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public int Level { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;

    public event System.Action<int> OnLevelUp;

    [Header("XP UI Elements")]
    [SerializeField] Image XPBar;
    [SerializeField] TextMeshProUGUI levelText;

    // Calculates xp required for next level up (xp is cumulative)
    public int XPToNext => Mathf.RoundToInt(80 * Mathf.Pow(Level, 1.5f));

    private void Update()
    {
        XPBar.fillAmount = (float)CurrentXP / XPToNext;
        levelText.text = "LEVEL " + Level.ToString();
    }

    public void AddXP(int amount)
    {
        CurrentXP += amount;
        while (CurrentXP >= XPToNext) // Use while, so lots of exp can give multiple levels
        {
            CurrentXP -= XPToNext;
            Level++;
            OnLevelUp?.Invoke(Level);
        }
    }

    public void Apply()
    {
        
    }
}
