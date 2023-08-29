using Common;
using UnityEngine;

namespace Scripts.Sim1
{
    public class Home : MonoBehaviour
    {
        [SerializeField]
        private Ant antPrototype;

        [SerializeField]
        private AntConfig antConfig;

        [SerializeField]
        private float antSpawnInterval;

        [SerializeField]
        private int startCount;

        [SerializeField]
        [InspectorReadOnly]
        private float timeToSpawn = 0;

        private void Start()
        {
            for (int i = 0; i < startCount; i++)
            {
                SpawnAnt();
            }
        }

        private void Update()
        {
            timeToSpawn -= Time.deltaTime;
            if (timeToSpawn <= 0)
            {
                SpawnAnt();
            }
        }

        private void SpawnAnt()
        {
            Ant ant = Instantiate(antPrototype, transform.position, new Quaternion());
            ant.SetAntConfig(antConfig);

            ant.WalkDirection = Vector3.up.RotateZWithDegrees(UnityEngine.Random.Range(0f, 360f));

            timeToSpawn = antSpawnInterval;
        }
    }
}
