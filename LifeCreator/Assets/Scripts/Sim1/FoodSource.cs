using UnityEngine;

namespace Scripts.Sim1
{
    public class FoodSource : MonoBehaviour
    {
        [SerializeField]
        private Food foodPrototype;

        [SerializeField]
        private int foodCount;

        [SerializeField]
        private bool infinityFood = false;

        [SerializeField]
        private Collider2D foodSourceZone;
        public Collider2D FoodSourceZone => foodSourceZone;

        public void GrabFood(out Food food)
        {
            food = Instantiate(foodPrototype, transform.position, new Quaternion(), transform);
            foodCount--;
            if (foodCount == 0 && !infinityFood)
            {
                Destroy(gameObject);
            }
        }
    }
}
