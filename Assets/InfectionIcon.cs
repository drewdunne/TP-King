using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionIcon : MonoBehaviour
{

    
    public int quantityOfSteps;
    public float stepSize;
    private float startingPosition;
    private float endPosition = 182;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = -180.3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIconAttributes(int playerMaxHealth)
    {
        endPosition = 182;
        quantityOfSteps = playerMaxHealth;
        stepSize = 525 / quantityOfSteps;
    }
    

    public void OnInfectionUpdated(GameObject playerObj, int infectionDelta)
    {
        Vector3 distance = new Vector3(0, stepSize * playerObj.GetComponent<PlayerManager>().lastLoggedInfectionChange, 0);
        gameObject.GetComponent<Transform>().Translate(distance);
    }
}
