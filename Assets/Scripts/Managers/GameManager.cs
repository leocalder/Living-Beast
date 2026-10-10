using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject enemyObject;

    bool isSpawning = true;

    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    void Update()
    {

    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (isSpawning)
        {
            Instantiate(enemyObject, GenerateRandomSpawnPoint(), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4f, 8f));
        }
    }

    // Generates a random enemy spawn point around the screen's edges
    private Vector3 GenerateRandomSpawnPoint()
    {
        Vector2 viewportPoint;

        // Generates a random point along the viewport edges, stored in viewport space (0 to 1 in x and y-axes)
        if (Random.value > 0.5f)
        {
            viewportPoint = new Vector2(Random.value, Random.value > 0.5f ? 0f : 1f);
        }
        else
        {
            viewportPoint = new Vector2(Random.value > 0.5f ? 0f : 1f, Random.value);
        }

        Vector3 screenEdgePoint = GetRandomScreenEdgePoint(viewportPoint); // Uses viewport point to generate random screen edge point
        return screenEdgePoint;
    }

    // Fetches a random screen edge point in world position
    Vector2 GetRandomScreenEdgePoint(Vector2 viewportPoint)
    {
        return Camera.main.ViewportToWorldPoint(viewportPoint);
    }
}
