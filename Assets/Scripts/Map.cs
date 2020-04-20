using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public const float MAP_SIZE_X = 31;
    public const float MAP_SIZE_Y = 17.1f;

    public bool CollidingXBorder(float x)
    {
        return (x >= MAP_SIZE_X || x <= -MAP_SIZE_X);
    }

    public bool CollidingYBorder(float y)
    {
        return (y >= MAP_SIZE_Y || y <= -MAP_SIZE_Y);
    }
}
