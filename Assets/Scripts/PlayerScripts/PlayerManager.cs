﻿using System;
using UnityEngine;

enum ModelType {healthyModel, sickModel, deadModel}
public class PlayerManager : MonoBehaviour
{

    #region varDeclarations
    public int score;
    public int infectionAmt;
    public int maxHealth;
    public bool isMaskEquipped;
    public GameObject equippedMaskPrefab;
    private GameObject maskObj;
    public int lastLoggedInfectionChange;
    private MaskManager mask;
    private UIManager uiManager;
    private SpriteRenderer spriteR;
    private Sprite[] sprites;
    
#endregion
    #region eventHandlers
    public delegate void InfectionChangedEventHandler (GameObject player, EventArgs args);

    public event InfectionChangedEventHandler InfectionUpdated;
#endregion
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        score = 0;
    }
    public void IncreaseScore(int pointValue)
    {
        score += pointValue;
        uiManager.UpdateScore(score);
    }
    public void ModifyInfection(int deltaInfection)
    {
        if (infectionAmt + deltaInfection < 0)
        {
            deltaInfection = -infectionAmt;
        }
        infectionAmt += deltaInfection;
        lastLoggedInfectionChange = deltaInfection;
        OnInfectionChanged(gameObject, EventArgs.Empty);
        UpdatePlayerSprite();
    }
    public void EquipPlayerMask()
    {
        isMaskEquipped = true;
        maskObj = Instantiate(equippedMaskPrefab, transform.position, transform.rotation);
        mask = maskObj.GetComponent<MaskManager>();
    }
    /// <summary>
    /// Loads All Sprites in Folder of specified difficulty. This includes subfolders.
    /// </summary>
    /// <param name="difficulty"></param>
    public void LoadPlayerSprites(Difficulty difficulty)
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Player_" + difficulty.ToString());

        spriteR.sprite = sprites[0];
    }
    public void DestroyPlayerColliders()
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        if (isMaskEquipped == true)
        {
            Debug.Log("MASKEQUIPPED TEST" + isMaskEquipped.ToString());
            GameObject.Find("Equipped Mask(Clone)").GetComponent<MaskManager>().DestroyMask();
        }
    }
    private void UpdatePlayerSprite()
    {
        if (infectionAmt / maxHealth < .5)
        {
                spriteR.sprite = sprites[(int)ModelType.healthyModel];
        }
        else if (infectionAmt < maxHealth)
        {
                spriteR.sprite = sprites[(int)ModelType.sickModel];
        }
        else
        {
            spriteR.sprite = sprites[(int)ModelType.deadModel];
        }

    }
    public void OnMaskEquipped(GameObject source, EventArgs e)
    {
        isMaskEquipped = true;
    }
    public void OnDestroyingMask(GameObject source, EventArgs e)
    {
        isMaskEquipped = false;
    }
    protected virtual void OnInfectionChanged (GameObject playerObj, EventArgs args)
    {
        if (InfectionUpdated != null)
            InfectionUpdated(playerObj, args);
    }
}
