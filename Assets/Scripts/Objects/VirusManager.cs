using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VirusManager : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject playerObj;
    private int minSpeed = 0;
    private int maxSpeed = 10;
    private int totalSpeed;
    private int randomSpeed_x;
    private int randomSpeed_y;


    private void Start()
    {
        playerObj = GameObject.Find("Player(Clone)");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        MakeVirusMove();

    }


    private void Update()
    {
        //if (gameManager.gameActive == true)
        //{
        //    if (Rigidbody2D.velocity)
        //        MakeVirusMove();
        //}
    }

    private void MakeVirusMove()
    {
        do {
            randomSpeed_x = Random.Range(minSpeed, maxSpeed);
            randomSpeed_y = Random.Range(minSpeed, maxSpeed);
            totalSpeed = randomSpeed_x + randomSpeed_y;
        } while (totalSpeed < 3 || totalSpeed > 15);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(randomSpeed_x, randomSpeed_y), ForceMode2D.Impulse);
    }

}
