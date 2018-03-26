using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Managers
{
    public class Spawn_boss : MonoBehaviour
    {

        public GameObject enemy;                // Loups boss
        public Transform[] spawnPoints;         // Spawn forêt
        public GameManager gameManager;
        void Start()
        {

        }

        void Begin_Night()
        {
            int cycle = gameManager.GetCycle();
            if (cycle % 10 == 0)
            {
                //boss_healt = boss_health + cycle
                int rand_temp = Random.Range(1, 30);
                Invoke("Spawn", rand_temp);
            }
        }
        void Spawn()
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            // Create an instance of the boss    prefab at the randomly selected spawn point's position and rotation.
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        }
    }
}