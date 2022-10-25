using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StandartFeeding : MonoBehaviour
{
    float time_delta = 0.05f;
    GameObject food;

    float time_var = 0;

    IMap map;

    private void Start()
    {
        map = FindObjectsOfType<MonoBehaviour>().OfType<IMap>().First();
        food = Resources.Load("Food/StandartFood") as GameObject;
    }

    void FixedUpdate()
    {
        time_var += Time.fixedDeltaTime;
        if (time_var >= time_delta)
        {
            Instantiate(food, map.GetRandomPositionInArea(), new Quaternion(), gameObject.transform);
        }
    }
}
