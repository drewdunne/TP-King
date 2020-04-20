using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maximumPlayerSpeed;
    public float minimumPlayerSpeed; //A percentage of max speed
    public float deadSpeedZoneDistance;
    public float variableSpeedZoneDistance;
    public Map map;
    private Vector3 moveCommand;
    private bool isAgainstNorthWall;
    private bool isAgainstSouthWall;
    private bool isAgainstWestWall;
    private bool isAgainstEastWall;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        maximumPlayerSpeed = 11;
        map = GameObject.Find("Map").GetComponent<Map>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        variableSpeedZoneDistance = 5f;
        deadSpeedZoneDistance = 1.5f;
        minimumPlayerSpeed = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameActive == true)
        {
            GetMoveCommand();
            RotateTowardsMouse();
            AdjustIfAgainstWall();
            UpdatePosition();
        }
    }


    private void GetMoveCommand()
    {
        moveCommand = GetMouseCoordinates();
    }

    private void UpdatePosition()
    {
        float variableSpeedZoneSize = variableSpeedZoneDistance - deadSpeedZoneDistance;
        float mouseDistanceFromPlayer = Vector3.Distance(moveCommand, transform.position);
        float percentageOfMaxSpeed = mouseDistanceFromPlayer / variableSpeedZoneSize;

        if (percentageOfMaxSpeed < minimumPlayerSpeed)
        {
            percentageOfMaxSpeed = minimumPlayerSpeed;
        }

        if (mouseDistanceFromPlayer < deadSpeedZoneDistance)
        { moveCommand = transform.position; }
        else if (percentageOfMaxSpeed < 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveCommand, maximumPlayerSpeed * Time.deltaTime * percentageOfMaxSpeed);
        }
        else
        { transform.position = Vector3.MoveTowards(transform.position, moveCommand, maximumPlayerSpeed * Time.deltaTime); }
        
    }

    private Vector3 GetMouseCoordinates()
    {
        Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseCoordinates.z = transform.position.z;

        return mouseCoordinates;
    }
  
    private void RotateTowardsMouse()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, moveCommand - transform.position);
    }

    private void AdjustIfAgainstWall()
    {
        if(isAgainstNorthWall && moveCommand.y > transform.position.y)
        {
            moveCommand.y = transform.position.y;
        }

        else if (isAgainstSouthWall && moveCommand.y < transform.position.y )
        {
            moveCommand.y = transform.position.y;
        }

        if(isAgainstWestWall && moveCommand.x < transform.position.x)
        {
            moveCommand.x = transform.position.x;
        }

        else if (isAgainstEastWall && moveCommand.x > transform.position.x)
        {
            moveCommand.x = transform.position.x;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision Detected " + other.gameObject.name);
        if (other.gameObject.CompareTag("NBoundary"))
        {
            isAgainstNorthWall = true;
        }

        if(other.gameObject.CompareTag("SBoundary"))
        {
            isAgainstSouthWall = true;
        }

        if (other.gameObject.CompareTag("EBoundary"))
        {
            isAgainstEastWall = true;
        }

        if (other.gameObject.CompareTag("WBoundary"))
        {
            isAgainstWestWall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NBoundary"))
        {
            isAgainstNorthWall = false;
        }

        if (other.gameObject.CompareTag("SBoundary"))
        {
            isAgainstSouthWall = false;
        }

        if (other.gameObject.CompareTag("EBoundary"))
        {
            isAgainstEastWall = false;
        }

        if (other.gameObject.CompareTag("WBoundary"))
        {
            isAgainstWestWall = false;
        }
    }


}
