using UnityEngine;

public class Death : MonoBehaviour
{
    GameObject player;

    [Header("Prefabs")]
    [SerializeField] GameObject corpse;
    [SerializeField] GameObject deathBloodFloorDecal1;
    [SerializeField] GameObject deathBloodFloorDecal2;
    [SerializeField] GameObject deathBloodFloorDecal3;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void _Death()
    {
        PlayerLevel playerLevel = player.GetComponent<PlayerLevel>();
        playerLevel.AddXP(25);
        GenerateDeathBloodFloorDecal();
        GameObject corpseObject = Instantiate(corpse, transform.position, Quaternion.identity);
        CorpseKnockback(corpseObject);

        Destroy(gameObject);
    }

    // Knocks back the corpse slightly when the enemy dies.
    // Creates a more impactful death
    void CorpseKnockback(GameObject corpseObject)
    {
        if (corpseObject.TryGetComponent<Movement>(out Movement movement))
        {
            movement.TakeKnockback(-transform.up, 25f, 15f, 0.4f);
        }
        else
        {
            Debug.LogError("Attach Movement component to corpse object -> " + corpse.name);
        }
    }

    // Projects blood decal on floor after death, positioned slightly behind the corpse
    void GenerateDeathBloodFloorDecal()
    {
        int rand = Random.Range(1, 4);
        GameObject randomDecal;
        if (rand == 1)
        {
            randomDecal = deathBloodFloorDecal1;
        }
        else if (rand == 2)
        {
            randomDecal = deathBloodFloorDecal2;
        }
        else if (rand == 3)
        {
            randomDecal = deathBloodFloorDecal3;
        }
        else
        {
            randomDecal = null;
        }

        GameObject blood = Instantiate(randomDecal, transform.position - transform.up * 1.4f, Quaternion.identity);
        blood.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        float bloodScale = Random.Range((blood.transform.localScale.x + 0.4f), blood.transform.localScale.x + 0.6f);
        blood.transform.localScale = new Vector3(bloodScale, bloodScale, 0);
    }
}
