using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class StandartFeeding : MonoBehaviour
{
    [SerializeField]
    List<GameObject> all_food = new();
    [SerializeField]
    float time_delta = 0.05f;
    [SerializeField]
    GameObject food;
    [SerializeField]
    float time_var = 0;
    [SerializeField]
    IMap map;
    [SerializeField]
    int max_food_count = 200;

    private void Start()
    {
        map = FindObjectsOfType<MonoBehaviour>().OfType<IMap>().First();
        food = Resources.Load("Food/StandartFood") as GameObject;
    }

    void FixedUpdate()
    {
        time_var += Time.fixedDeltaTime;
        if (time_var >= time_delta && all_food.Count <= max_food_count)
        {
            all_food.Add(Instantiate(food, map.GetRandomPositionInArea(), new Quaternion(), gameObject.transform));
            time_var = 0;
        }
        all_food = all_food.Where(x => x != null).ToList();
    }
}
