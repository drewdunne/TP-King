using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public GameObject player;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameActive == true)
        {
            gameObject.transform.position = player.transform.position + new Vector3(0, 0, -40);
            gameObject.GetComponent<Camera>().orthographicSize = 8;
        }
    }
    public void AttachCameraToPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

}

