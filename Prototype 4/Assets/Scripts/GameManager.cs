using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject instructionScreen;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject titleSettingsScreen;
    [SerializeField] GameObject ingameSettingsScreen;
    [SerializeField] GameObject ingameSettingsButtonCover;

    [SerializeField] Button instructionButton;
    [SerializeField] Button exitInstructionButton;
    [SerializeField] Button titleSettingsButton;
    [SerializeField] Button ingameSettingsButton;
    [SerializeField] Button exitTitleSettingsButton;
    [SerializeField] Button exitIngameSettingsButton;
    [SerializeField] Button ingameRestartButton;

    SpawnManager spawnManager;
    SoundManager soundManager;

    public TextMeshProUGUI livesText;
    public int lives;

    // Game status variables
    public bool gameIsOn = false;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        soundManager = GameObject.Find("Game Manager").GetComponent<SoundManager>();
        instructionButton.onClick.AddListener(OpenInstructionsScreen);
        titleSettingsButton.onClick.AddListener(OpenTitleSettingsScreen);
        ingameSettingsButton.onClick.AddListener(OpenIngameSettingsScreen);
    }

    public void StartGame(int difficulty)
    {
        spawnManager.bossRound = difficulty;
        gameIsOn = true;
        titleScreen.SetActive(false);
        ingameSettingsButtonCover.SetActive(true);
        UpdateLives(3);
        int randomPowerup = Random.Range(0, spawnManager.powerupPrefabs.Length);
        spawnManager.SpawnEnemyWave(spawnManager.waveNumber);
        spawnManager.roundText.text = "" + spawnManager.waveNumber;
        Instantiate(spawnManager.powerupPrefabs[randomPowerup], spawnManager.GenerateSpawnPosition(), spawnManager.powerupPrefabs[randomPowerup].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // Open the Game Over Screen when the player falls off the platform 
        if (gameOver)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void UpdateLives(int livesChange)
    {
        lives += livesChange;
        livesText.text = "Lives: " + lives;
        if (lives <= 0)
        {
            gameIsOn = false;
            gameOver = true;
        }
    }

    void OpenInstructionsScreen()
    {
        titleScreen.SetActive(false);
        instructionScreen.SetActive(true);
        exitInstructionButton.onClick.AddListener(ExitInstructionScreen);
    }

    void ExitInstructionScreen()
    {
        instructionScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    void OpenTitleSettingsScreen()
    {
        titleScreen.SetActive(false);
        titleSettingsScreen.SetActive(true);
        exitTitleSettingsButton.onClick.AddListener(ExitTitleSettingsScreen);
    }

    void ExitTitleSettingsScreen()
    {
        titleSettingsScreen.SetActive(false);
        titleScreen.SetActive(true);
    }

    void OpenIngameSettingsScreen()
    {
        gameIsOn = false;
        ingameSettingsScreen.SetActive(true);
        exitIngameSettingsButton.onClick.AddListener(ExitIngameSettingsScreen);
        ingameRestartButton.onClick.AddListener(RestartGame);
    }

    void ExitIngameSettingsScreen()
    {
        ingameSettingsScreen.SetActive(false);
        gameIsOn = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        soundManager.SaveSoundSettings();
    }
}
