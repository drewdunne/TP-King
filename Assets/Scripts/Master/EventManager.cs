using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

#region varDeclarations

    public GameObject gameManagerObj;
    public GameObject infectionIconObj;
    public GameObject canvas;
    private UIManager uiManager;
    private ItemManager itemManager;
    private GameManager gameManager;
    private PlayerManager player;
    private MaskManager maskManager;
    private DifficultyManager difficultyManager;
    private SpawnManager spawnManager;
    private InfectionIcon infectionIcon;
    private SoundManager soundManager;

#endregion
    // Start is called before the first frame update
void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        soundManager = soundManager.GetComponent<SoundManager>();
        uiManager = canvas.GetComponent<UIManager>();
        difficultyManager = gameManagerObj.GetComponent<DifficultyManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        SubscriberSetup_PlayerCreated();
    }

    #region Subscriber Management
    private void SubscriberSetup_PlayerCreated()
    {
        gameManager.PlayerCreated += OnPlayerCreated;
        gameManager.PlayerCreated += difficultyManager.OnPlayerCreated;
        gameManager.PlayerCreated += uiManager.OnPlayerCreated;
        gameManager.PlayerCreated += spawnManager.OnPlayerCreated;
    }

    private void SubscribeSetup_MaskEquipped()
    {
        itemManager.MaskEquipped += player.OnMaskEquipped;
        itemManager.MaskEquipped += uiManager.OnMaskEquipped;
        itemManager.MaskEquipped += difficultyManager.OnMaskEquipped;
        itemManager.MaskEquipped += OnMaskEquipped;
    }

    private void SubscriberSetup_MaskDurabilityUpdated()
    {
        maskManager.MaskDurabilityUpdated += uiManager.OnMaskDurabilityUpdated;
        SubscriberSetup_DestroyingMask();
    }

    private void SubscriberSetup_DestroyingMask()
    {
        maskManager.DestroyingMask += uiManager.OnDestroyingMask;
        maskManager.DestroyingMask += player.OnDestroyingMask;
    }

    private void SubscriberSetup_InfectionUpdated()
    {
        player.InfectionUpdated += uiManager.OnInfectionUpdated;
        player.InfectionUpdated += infectionIcon.OnInfectionUpdated;
    }
    #endregion

    #region EventHandlers
    public void OnPlayerCreated(GameObject playerObj, EventArgs args)
    {
        itemManager = playerObj.GetComponent<ItemManager>();
        player = playerObj.GetComponent<PlayerManager>();
        infectionIcon = infectionIconObj.GetComponent<InfectionIcon>();
        SubscribeSetup_MaskEquipped();
        SubscriberSetup_InfectionUpdated();
    }

    public void OnMaskEquipped (GameObject source, EventArgs e)
    {
        maskManager = GameObject.Find("Equipped Mask(Clone)").GetComponent<MaskManager>();
        SubscriberSetup_MaskDurabilityUpdated();
    }
    #endregion
}
