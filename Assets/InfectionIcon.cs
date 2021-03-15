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
        stepSize = (endPosition - startingPosition) / quantityOfSteps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIconAttributes(int playerMaxHealth)
    {
        endPosition = 182;
        quantityOfSteps = playerMaxHealth;
    }
    

    public void OnInfectionUpdated(GameObject playerObj, int infectionDelta)
    {
        float amountToMove = stepSize * infectionDelta;
        Vector3 currentPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
        Vector3 newPosition = new Vector3(currentPosition.x + amountToMove, currentPosition.y, currentPosition.z);

        gameObject.GetComponent<RectTransform>().anchoredPosition = newPosition;

        //gameObject.GetComponent<Transform>().Translate(distance); (depricated, was not translating by correct value for some reason)
    }
}
