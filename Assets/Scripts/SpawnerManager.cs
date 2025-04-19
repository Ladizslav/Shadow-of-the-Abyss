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
            SpawnEnemy(spawnPoint);  // Spawnuje nového nepøítele
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
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, this.transform); // Nastavení rodièe na tento objekt

        activeEnemies++;  // Zvyšujeme poèet aktivních nepøátel
        activeEnemyList.Add(newEnemy);
    }

    public void EnemyDestroyed(GameObject enemy)
    {
        activeEnemies--;  // Snížíme poèet aktivních nepøátel
        activeEnemyList.Remove(enemy);  // Odebereme znièeného nepøítele z listu

        // Pokud jsou všichni nepøátelé znièeni, pøièteme bod do PlayerScore
        if (activeEnemies == 0 && spawnedEnemies >= totalEnemiesToSpawn)
        {
            // Pøidáme bod do PlayerScore
            PlayerScore.Instance.AddPoint();
        }
    }
}
