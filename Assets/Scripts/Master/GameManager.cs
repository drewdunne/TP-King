using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public enum Difficulty { Easy, Medium, Hard };

public class GameManager : MonoBehaviour
{
    public bool gameActive;
    public Button restartButton;
    public GameObject playerPrefab;
    public GameObject spawnManagerObj;
    public GameObject canvasObj;
    public GameObject playerObj;
    public GameObject[] difficultyButtons;
    public TextMeshProUGUI gameOverText;
    private Difficulty difficulty;
    private UIManager uiManager;
    private PlayerManager playerManager;
    private SpawnManager spawnManager;
    private CameraBehavior cameraView;
    private DifficultyManager difficultyManager;
    private ScoreTracker scoreTracker;

    public delegate void PlayerCreatedEventHandler(GameObject playerObj, EventArgs args);

    public event PlayerCreatedEventHandler PlayerCreated;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Resetting High Score");
        gameActive = false;
        cameraView = GameObject.Find("Camera").GetComponent<CameraBehavior>();
        spawnManager = spawnManagerObj.GetComponent<SpawnManager>();
        difficultyManager = gameObject.GetComponent<DifficultyManager>();
        uiManager = canvasObj.GetComponent<UIManager>();
        scoreTracker = GameObject.Find("Score Tracker").GetComponent<ScoreTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive == true)
        {
            IsGameOverCheck();
        }

        //
    }

#region DifficultyButtons

    public void ButtonDifficultyEasy()
    {
        difficulty = Difficulty.Easy;
        InitializeGameplay();
    }
    public void ButtonDifficultyNormal()
    {
        difficulty = Difficulty.Medium;
        InitializeGameplay();
    }
    public void ButtonDifficultyHard()
    {
        difficulty = Difficulty.Hard;
        InitializeGameplay();
    }

#endregion


#region Start/End Functions

    private void InitializeGameplay()
    {
        gameActive = true;
        difficultyManager.SetDifficultyVariables(difficulty); 
        DeactivateDifficultyUI();
        playerObj = Instantiate(playerPrefab);
        OnPlayerCreated(playerObj, EventArgs.Empty);
        playerManager = playerObj.GetComponent<PlayerManager>();
        playerManager.LoadPlayerSprites(difficulty);
        cameraView.AttachCameraToPlayer(playerObj);
    }
    private void DeactivateDifficultyUI()
    {
        foreach (GameObject button in difficultyButtons)
        {
            button.SetActive(false);
        }
    }
    private void EndGameplay()
    {
        gameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        playerManager.DestroyPlayerColliders();
        spawnManager.EndSpawners();
    }

        

#endregion

    private void RestartGame()
    {
        uiManager.DeactivateGameplayUI();
        Destroy(playerObj);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        uiManager.highScoreObj.SetActive(true);
        UpdateHighScore();
    }
    private void IsGameOverCheck()
    {
        if (playerManager.infectionAmt >= playerManager.maxHealth)
        {
            EndGameplay();
        }
    }

    private void UpdateHighScore()
    {
        if (scoreTracker.highScore < playerManager.score)
        {
            scoreTracker.highScore = playerManager.score;
        }

        uiManager.UpdateHighScoreText(scoreTracker.highScore);
    }

    protected virtual void OnPlayerCreated(GameObject playerObj, EventArgs args)
    {
        if (PlayerCreated != null)
            PlayerCreated(playerObj, EventArgs.Empty);
    }

    

    
}