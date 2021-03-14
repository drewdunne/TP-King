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
