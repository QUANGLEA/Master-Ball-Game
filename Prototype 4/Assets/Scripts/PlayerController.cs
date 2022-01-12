using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Sound Effect
    AudioSource knockbackSound;
    AudioSource smashSound;
    AudioSource rocketSound;

    // Speed and Strength
    private float speed = 8.0f;
    private float powerUpStrength = 15.0f;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerupIndicator;

    // Powerup variables
    public bool hasPowerup = false;
    public PowerUpType currentPowerUp = PowerUpType.None;

    // GameObjects and Coroutine
    public GameObject rocketPrefab;
    GameObject tmpRocket;
    Coroutine powerupCountdown;
    GameManager gameManager;

    // Variables for smash powerup
    float hangTime = 5.0f;
    float smashSpeed = 10.0f;
    float explosionForce = 200.0f;
    float explosionRadius = 10.0f;
    bool smashing = false;
    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        knockbackSound = GameObject.Find("Knockback Sound").GetComponent<AudioSource>();
        smashSound = GameObject.Find("Smash Sound").GetComponent<AudioSource>();
        rocketSound = GameObject.Find("Rocket Sound").GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsOn)
        {
            float forwardInput = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        }
        else
        {
            playerRb.velocity = Vector3.zero;
        }

        // If the player falls off the platform then the game is over
        if (transform.position.y < -10)
        {
            gameManager.UpdateLives(-1);
            
            if (gameManager.lives > 0)
            {
                transform.position = Vector3.zero;
                playerRb.velocity = Vector3.zero;
                currentPowerUp = PowerUpType.None;
                powerupIndicator.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // If the player picks up the rocket powerup, they can shoot rockets when pressed space
        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play(0);
            LaunchRockets();
        }

        // If the player picks up the smash powerup, they can press space to jump up and down to knock back enemies
        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            smashSound.Play(0);
            StartCoroutine(Smash());
        }
    }

    // This function is to check when the player touches a powerup and then initiate the powerup timer
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerUp = other.gameObject.GetComponent<Powerup>().powerUpType;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            powerupCountdown = StartCoroutine(PowerupCountdownRoutine());
        }    
    }

    IEnumerator Smash()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Store the y position before taking off
        floorY = transform.position.y;

        // Calculate the amount of time we will go up
        float jumpTime = +hangTime;

        while (Time.time < jumpTime)
        {
            // Move the player up while still keeping their x velocity
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        // Now move the player down
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        // Cycle through all enemiies.
        for (int i = 0; i < enemies.Length; i++)
        {
            // Apply an explision force that originates from our position
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }

        // We are no longer smashing, set the boolean to false
        smashing = false;
    }

    // This function is activated when the player picks up a powerup
    // There will be an indicator which tells the player when the powerup is still in play 
    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        currentPowerUp = PowerUpType.None;
        powerupIndicator.SetActive(false);
    }

    // This function is activated when the player picks up the knockback powerup and collides with an enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerUp == PowerUpType.Pushback)
        {
            knockbackSound.Play(0);
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.transform.position - transform.position;

            // Pushes the enemy away with greater force
            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse); // This line is to 
            Debug.Log("Player collided with " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());
        }
    }

    // This function launches rockets at enemies when the rocket powerup is picked up by the player
    void LaunchRockets()
    {
        // For each enemy on the platform, the player shoots out a rocket
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);

            // Have the rocket follows the enemies
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform);
        }
    }
}
