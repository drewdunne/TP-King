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
        uiManager.LoadSplashScreens();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive == true)
        {
            IsGameOverCheck();
        }

    }

#region DifficultyButtons

    public void ButtonDifficultyEasy()
    {
        difficulty = Difficulty.Easy;
        difficultyButtons[0].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        difficultyButtons[1].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        difficultyButtons[2].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
    public void ButtonDifficultyNormal()
    {
        difficulty = Difficulty.Medium;
        difficultyButtons[0].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        difficultyButtons[1].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        difficultyButtons[2].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
    }
    public void ButtonDifficultyHard()
    {
        difficulty = Difficulty.Hard;
        difficultyButtons[0].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        difficultyButtons[1].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        difficultyButtons[2].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

#endregion


#region Start/End Functions

    public void InitializeGameplay()
    {
        gameActive = true;
        difficultyManager.SetDifficultyVariables(difficulty); 
        uiManager.UnloadMainMenuScreen();
        playerObj = Instantiate(playerPrefab);
        OnPlayerCreated(playerObj, EventArgs.Empty);
        playerManager = playerObj.GetComponent<PlayerManager>();
        playerManager.LoadPlayerSprites(difficulty);
        cameraView.AttachCameraToPlayer(playerObj);
    }
    
    private void EndGameplay()
    {
        gameActive = false;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        playerManager.DestroyPlayerColliders();
        spawnManager.EndSpawners();
    }

    private void RestartGame()
    {
        uiManager.DeactivateGameplayUI();
        Destroy(playerObj);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        uiManager.highScoreObj.SetActive(true);
        UpdateHighScore();
    }

        
#endregion

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