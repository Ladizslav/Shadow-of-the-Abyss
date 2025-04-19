using System.Collections;
using UnityEngine;
using TMPro;

public class EndlessModeManager : MonoBehaviour
{
    public static EndlessModeManager Instance;

    public GameObject[] enemyPrefabs;
    public GameObject healingTreePrefab;
    public Transform[] spawnPoints;
    public Transform healingTreeSpawnPoint;

    public int baseEnemies = 5;
    public int enemiesIncrement = 1;
    public float timeBetweenWaves = 10f;
    public float bossWaveDelay = 30f;

    public TMP_Text waveText;
    public TMP_Text enemiesText;
    public TMP_Text highscoreText;

    private int currentWave = 0;
    private int enemiesToDefeat = 0;
    private int enemiesDefeated = 0;
    private int highscore = 0;
    private bool isPreparingWave = false;
    private GameObject currentHealingTree;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadHighscore();
            UpdateUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {


        isPreparingWave = true;
        float delay = currentWave % 3 == 0 ? bossWaveDelay : timeBetweenWaves;
        yield return new WaitForSeconds(delay);

        currentWave++;
        enemiesToDefeat = baseEnemies + (currentWave/2 * enemiesIncrement);
        enemiesDefeated = 0;
        UpdateUI();

        if (currentWave % 3 == 0)
        {
            SpawnHealingTree();
        }

        for (int i = 0; i < enemiesToDefeat; i++)
        {
            SpawnSingleEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        isPreparingWave = false;
    }

    void SpawnSingleEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyEndless enemyScript = enemy.GetComponent<EnemyEndless>();
        enemyScript.InitializeForWave(currentWave);
    }

    public void EnemyDefeated()
    {
        enemiesDefeated++;
        UpdateUI();

        if (enemiesDefeated >= enemiesToDefeat && !isPreparingWave)
        {
            if (currentWave > highscore)
            {
                highscore = currentWave;
                SaveHighscore();
            }
            StartCoroutine(StartNextWave());
        }
    }

    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = $"Wave: {currentWave}";

        if (enemiesText != null)
            enemiesText.text = $"Defeated: {enemiesDefeated}/{enemiesToDefeat}";

        if (highscoreText != null)
            highscoreText.text = $"Record: {highscore}";
    }

    void SpawnHealingTree()
    {
        if (currentHealingTree != null)
        {
            Destroy(currentHealingTree);
        }

        if (healingTreePrefab != null && healingTreeSpawnPoint != null)
        {
            currentHealingTree = Instantiate(
                healingTreePrefab,
                healingTreeSpawnPoint.position,
                healingTreeSpawnPoint.rotation
            );
        }
    }

    void SaveHighscore()
    {
        PlayerPrefs.SetInt("EndlessHighscore", highscore);
        PlayerPrefs.Save();
    }

    void LoadHighscore()
    {
        highscore = PlayerPrefs.GetInt("EndlessHighscore", 0);
    }
}