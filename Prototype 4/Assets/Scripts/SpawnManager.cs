using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] powerupPrefabs;
    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;

    GameManager gameManager;

    public TextMeshProUGUI roundText;

    // Enemy variables
    private float spawnRange = 9.0f;
    private int enemyCount;
    public int waveNumber = 1;
    public int bossRound;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // While the game is still playing
        if (gameManager.gameIsOn)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            // Checks if there are no enemies on the platform
            if (enemyCount == 0)
            {
                waveNumber++;
                roundText.text = "" + waveNumber;

                // Spawn a boss every x number of waves
                if (waveNumber % bossRound == 0)
                {
                    SpawnBossWave(waveNumber);
                }
                else
                {
                    SpawnEnemyWave(waveNumber);
                }

                // Updated to select a random powerup prefab for the Medium Challenge
                int randomPowerup = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
            }
        }
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int index = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[index], GenerateSpawnPosition(), miniEnemyPrefabs[index].transform.rotation);
        }
    }

    public Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return spawnPos;
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;

        // We don't want to divide by 0!
        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }

        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);

        // Put the value of 'miniEnemysToSpawn' back into 'miniEnemySpawnCount' of the Enemy script
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;

    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }
}
