using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int totalEnemiesToSpawn = 10;
    public int maxActiveEnemies = 3;

    private int activeEnemies = 0;
    private int spawnedEnemies = 0;
    private bool allEnemiesSpawned = false;

    private List<GameObject> activeEnemyList = new List<GameObject>();

    public void TrySpawnEnemy(Transform spawnPoint)
    {
        if (activeEnemies < maxActiveEnemies && spawnedEnemies < totalEnemiesToSpawn)
        {
            SpawnEnemy(spawnPoint);  // Spawnuje nov�ho nep��tele
            spawnedEnemies++;

            if (spawnedEnemies >= totalEnemiesToSpawn)
            {
                allEnemiesSpawned = true;
            }
        }
        else
        {
            Debug.Log("Cannot spawn new enemy, either max active reached or all enemies are spawned.");
        }
    }

    private void SpawnEnemy(Transform spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, this.transform); // Nastaven� rodi�e na tento objekt

        activeEnemies++;  // Zvy�ujeme po�et aktivn�ch nep��tel
        activeEnemyList.Add(newEnemy);
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies--;  // Sn��me po�et aktivn�ch nep��tel
        activeEnemyList.Remove(enemy);  // Odebereme zni�en�ho nep��tele z listu

        // Pokud jsou v�ichni nep��tel� zni�eni, p�i�teme bod do PlayerScore
        if (activeEnemies == 0 && spawnedEnemies >= totalEnemiesToSpawn)
        {
            // P�id�me bod do PlayerScore
            PlayerScore.Instance.AddPoint();
        }
    }
}
