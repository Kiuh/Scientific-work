using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartMap : MonoBehaviour, IMap
{
    [SerializeField]
    Transform left_wall;
    [SerializeField]
    Transform up_wall;
    [SerializeField]
    Transform right_wall;
    [SerializeField]
    Transform down_wall;
    [SerializeField]
    float wall_thikness;
    public Vector2 GetRandomPositionInArea()
    {
        float x = Random.Range(left_wall.position.x + wall_thikness / 2, right_wall.position.x - wall_thikness / 2);
        float y = Random.Range(up_wall.position.y - wall_thikness / 2, down_wall.position.y + wall_thikness / 2);
        return new Vector2(x, y);
    }
}
