using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public GameObject equippedMask;
    private SpawnManager spawnManager;
    private PlayerManager player;
    private UIManager uiManager;
    private int tpValue;
    private int gtpValue;
    private int virusValue;
    private int sanitizerValue;
    private GameObject maskObj;
    public float maxMaskDurability;

    public delegate void MaskEquippedEventHandler(GameObject source, EventArgs args);
    public event MaskEquippedEventHandler MaskEquipped;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        sanitizerValue = -5;
        virusValue = 1;
        tpValue = 1;
        gtpValue = 5;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.name)
        {
            case ("Virus(Clone)"):
                {
                    player.ModifyInfection(virusValue);
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.name)
        {
            case ("Virus(Clone)"):
                {
                    player.ModifyInfection(virusValue);
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
            case ("Toilet Paper(Clone)"):
                {
                    player.IncreaseScore(tpValue);
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
            case ("Golden Toilet Paper(Clone)"):
                {
                    player.IncreaseScore(gtpValue);
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
            case ("Sanitizer(Clone)"):
                {
                    player.ModifyInfection(sanitizerValue);
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
            case ("Mask(Clone)"):
                {
                    if (player.isMaskEquipped == false)
                    {
                        maskObj = Instantiate(equippedMask, transform.position, transform.rotation); //TODO: Add equipped mask to game object list under player prefab
                        MaskManager mask = maskObj.GetComponent<MaskManager>();
                    }
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    OnMaskEquipped(maskObj, EventArgs.Empty);
                    Destroy(other.gameObject);
                    break;
                }
            case ("Lysol(Clone)"):
                {
                    GameObject[] allViruses = GameObject.FindGameObjectsWithTag("Virus");
                    
                    foreach (GameObject virus in allViruses)
                    {
                        spawnManager.RemoveItemFromActiveItemsList(virus);
                        Destroy(virus);
                    }
                    spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
                    Destroy(other.gameObject);
                    break;
                }
            default:
                break;
        }
        spawnManager.RemoveObjectFromList(other.gameObject);
    }

    protected virtual void OnMaskEquipped(GameObject mask, EventArgs e)
    {
        if (MaskEquipped != null)
            MaskEquipped(maskObj, e);
    }


}

