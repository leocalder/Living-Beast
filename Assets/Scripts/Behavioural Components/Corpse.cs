using Unity.VisualScripting;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    GameObject player;

    [Header("Stats")]
    [SerializeField] int expRewardUponConsumption;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void GetEaten()
    {
        PlayerLevel playerLevel = player.GetComponent<PlayerLevel>();
        playerLevel.AddXP(expRewardUponConsumption);

        Destroy(gameObject);
    }
}
