using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarderEnemy : MonoBehaviour
{
    float speed = 6.0f;
    Rigidbody enemyRb;
    GameObject player;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsOn)
        {
            Vector3 lookDirection = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed);
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
