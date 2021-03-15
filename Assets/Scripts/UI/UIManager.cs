using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public GameObject shoppingCartObj;
    public GameObject infectionBarObj;
    public GameObject maskDurabilityBarObj;
    public GameObject highScoreObj;
    public List<GameObject> MainMenuItems;
    public GameObject[] difficultyButtons;
    public GameObject[] difficultyButtonUnderlines;
    public GameObject splashScreen;
    public GameObject blackLayer;
    public GameObject instructionsScreen;


    private ShoppingCart shoppingCart;
    private SliderBar infectionBar;
    private SliderBar maskDurabilityBar;
    private ItemManager itemManager;

    // Start is called before the first frame update
    void Start()
    {
        infectionBar = infectionBarObj.GetComponent<SliderBar>();
        shoppingCart = shoppingCartObj.GetComponent<ShoppingCart>();
    }


    public void LoadSplashScreens()
    {
        StartCoroutine(LoadTitleScreen());
    }

    public IEnumerator LoadTitleScreen()
    {
        float splashScreenDuration = 3;
        bool timePassed = false;

        splashScreen.SetActive(true);
        blackLayer.SetActive(true);

        while (timePassed == false)
        {
            timePassed = true;
            yield return new WaitForSeconds(splashScreenDuration);
        }
        splashScreen.SetActive(false);
        LoadMainMenuScreen();

    }

    public void UnloadSplashScreens()
    {
        splashScreen.SetActive(false);
    }

    public void LoadMainMenuScreen()
    {
        foreach (GameObject menuItem in MainMenuItems)
        {
            menuItem.SetActive(true);
        }

        foreach (GameObject difficultyButton in difficultyButtons)
        {
            difficultyButton.SetActive(true);
            difficultyButton.transform.GetChild(0).gameObject.SetActive(true);
        }

        foreach (GameObject underline in difficultyButtonUnderlines)
        {
            underline.SetActive(false);
        }
    }
    public void UnloadMainMenuScreen()
    {
        foreach (GameObject menuItem in MainMenuItems)
        {
            menuItem.SetActive(false);
        }
    }

    public void LoadInstructionScreen()
    {
        UnloadMainMenuScreen();
        instructionsScreen.SetActive(true);
    }

    public void UnloadInstructionsScreen()
    {
        instructionsScreen.SetActive(false);
        LoadMainMenuScreen();
    }

    public void ActivateGameplayUI(float health)
    {
        infectionBarObj.SetActive(true);
        shoppingCartObj.SetActive(true);
        infectionBar = GameObject.Find("Infection Bar").GetComponent<SliderBar>();
        shoppingCart = GameObject.Find("Shopping Cart").GetComponent<ShoppingCart>();
        infectionBar.SetMaxValue(health);
    }


    private void ActivateMaskUI(float maxDurability)
    {
            maskDurabilityBarObj.SetActive(true);
            maskDurabilityBar = maskDurabilityBarObj.GetComponent<SliderBar>();
            maskDurabilityBar.SetMaxValue(maxDurability);
    }

    public void DeactivateGameplayUI()
    {
        infectionBarObj.SetActive(false);
        shoppingCartObj.SetActive(false);
    }
    
    
    public void UpdateScore(int score)
    {
        shoppingCart.UpdateShoppingCart(score);
    }

    public void UpdateHighScoreText(int score)
    {
        highScoreObj.GetComponent<TextMeshProUGUI>().text = $"High Score {score}";
    }

    public void OnMaskEquipped(GameObject source, EventArgs e)
    {
        if (maskDurabilityBarObj.activeInHierarchy == false)
            ActivateMaskUI(itemManager.maxMaskDurability);
        maskDurabilityBar.Refill();
    }

    public void OnPlayerCreated(GameObject playerObj, EventArgs args)
    {
        itemManager = playerObj.GetComponent<ItemManager>();
        ActivateGameplayUI(playerObj.GetComponent<PlayerManager>().maxHealth);
    }
    public void OnMaskDurabilityUpdated(GameObject source, EventArgs e, float durability)
    {
        maskDurabilityBar.SetValue(durability);
    }

    public void OnDestroyingMask(GameObject source, EventArgs e)
    {
        maskDurabilityBarObj.SetActive(false);
    }

    public void OnInfectionUpdated(GameObject playerObj, int infectionDelta)
    {
        infectionBar.SetValue(playerObj.GetComponent<PlayerManager>().infectionAmt);
        
    }

}
