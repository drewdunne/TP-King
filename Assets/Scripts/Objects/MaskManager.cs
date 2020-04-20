using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Subsystems;

public class MaskManager : MonoBehaviour
{

    public delegate void MaskDurabilityChangedEventHandler(GameObject source, EventArgs args, float durability);
    public event MaskDurabilityChangedEventHandler MaskDurabilityUpdated;

    public delegate void DestroyingMaskEventHandler(GameObject source, EventArgs args);
    public event DestroyingMaskEventHandler DestroyingMask;

    private GameObject playerObj;
    private SpawnManager spawnManager;
    private float durability;
    public float maxDurability;
  

    public float Durability
    {
        get => durability;
        set
        {
            if (value != durability)
            {
                durability = value;
                OnMaskDurabilityUpdated(durability);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player(Clone)");
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerObj.transform.position;
        transform.rotation = playerObj.transform.rotation;
    }

    public void DestroyMask()
    {
        Debug.Log("Destroying mask...");
        Destroy(gameObject);
        OnDestroyingMask();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Virus(Clone)")
        {
            spawnManager.RemoveItemFromActiveItemsList(other.gameObject);
            Destroy(other.gameObject);
            Durability--;
            if (Durability == 0)
            {
                Destroy(gameObject);
                OnDestroyingMask();
            }
        }
    }

    protected virtual void OnMaskDurabilityUpdated(float durability)
    {
        if(MaskDurabilityUpdated != null)
        {
            MaskDurabilityUpdated(gameObject, EventArgs.Empty, durability);
        }
    }

    protected virtual void OnDestroyingMask()
    {
        if(DestroyingMask != null)
        {
            DestroyingMask(gameObject, EventArgs.Empty);
        }
    }



}
