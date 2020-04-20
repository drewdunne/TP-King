using System;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    private int maxPlayerHealth;
    private int maxMaskDurability;
    private UIManager uIManager;


    public GameObject infectionIconObj;
    private InfectionIcon infectionIcon;

    void Start()
    {
        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        infectionIcon = infectionIconObj.GetComponent<InfectionIcon>();
}

    public void SetDifficultyVariables(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                {
                    maxPlayerHealth = 16;
                    maxMaskDurability = 40;
                    break;
                }
            case Difficulty.Medium:
                {
                    maxPlayerHealth = 8;
                    maxMaskDurability = 20;
                    break;
                }
            case Difficulty.Hard:
                {
                    maxPlayerHealth = 4;
                    maxMaskDurability = 10;
                    break;
                }
        }
        infectionIcon.SetIconAttributes(maxPlayerHealth);
    }


    public void OnPlayerCreated(GameObject playerObj, EventArgs e)
    {
        playerObj.GetComponent<PlayerManager>().maxHealth = maxPlayerHealth;
        playerObj.GetComponent<ItemManager>().maxMaskDurability = maxMaskDurability;
    }

    public void OnMaskEquipped(GameObject source, EventArgs e)
    {
        source.GetComponent<MaskManager>().maxDurability = maxMaskDurability;
        source.GetComponent<MaskManager>().Durability = maxMaskDurability;
    }
}
