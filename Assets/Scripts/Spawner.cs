using UnityEngine;

public class Spawner : MonoBehaviour
{
    private bool playerInRange = false;
    private float spawnDelay = 2f;
    private float spawnTimer = 0f;

    private void Update()
    {
        if (playerInRange)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnerManager spawnerManager = GetComponentInParent<SpawnerManager>();

                if (spawnerManager != null)
                {
                    spawnerManager.TrySpawnEnemy(transform);
                }

                spawnTimer = spawnDelay;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            spawnTimer = 0f; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
