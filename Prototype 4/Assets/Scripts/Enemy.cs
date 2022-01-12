using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    private GameObject player;
    private Rigidbody enemyRb;

    public bool isBoss = false;

    public float spawnInterval;
    private float nextSpawn;

    public int miniEnemySpawnCount;

    SpawnManager spawnManager;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsOn)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);

            if (isBoss)
            {
                if (Time.time > nextSpawn)
                {
                    nextSpawn = Time.time + spawnInterval;
                    spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
                }
            }
        }
        else
        {
            enemyRb.velocity = Vector3.zero;
        }
        
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
