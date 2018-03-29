using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Managers
{
    public class Spawn_wolf : MonoBehaviour
    {

        public GameObject enemy_classic;        // Loups Normaux
        public GameObject enemy_water;          // Loups lacs
        public GameObject enemy_ice;            // Loups montagne
        public GameObject boss;
        public Transform[] spawnPoints;         // Spawn classique
        public Transform[] spawnLac;             // Spawn lac
        public Transform[] spawnMountain;        // Spawn montagne
        public GameManager gameManager;
        public int Cycle;
        private List<GameObject> spawnedWolfs = new List<GameObject>();

        private int number_wolf;
        private int number_wolf_classic;
        private int number_wolf_water = 0;
        private int number_wolf_ice = 0;

        bool spawnboss;

        private void Awake()
        {
            spawnboss = false;
        }
        public void Begin_Night()
        {
            spawnboss = false;
            Cycle = GameManager.instance.GetCycle();
            // + 5 loups à chaque vagues
            if (Cycle <= 20)
            {
                number_wolf_classic = Cycle * 5;
            }
            else
            {
                number_wolf_classic = 100;
            }

            if (Cycle <= 26 && Cycle >= 6)
            {
                number_wolf_water ++;
            }
            if (Cycle <= 31 && Cycle >= 11)
            {
                number_wolf_ice ++;
            }
            if((Cycle % 10) == 0)
            {
                Debug.LogError("SPawn bosse");
                spawnboss = true;
            }
            number_wolf = number_wolf_classic + number_wolf_ice + number_wolf_water;
            if (spawnboss)
            {
                number_wolf = number_wolf + 1;
            }

            GameOverManager.instance.WolvesAliveInRound.Set(number_wolf);

            Spawn(number_wolf_classic, number_wolf_water, number_wolf_ice, spawnboss);
        }
        public void WolfDeath(GameObject wolf = null)
        {
            if (wolf == null)
            {
                Destroy(spawnedWolfs[spawnedWolfs.Count - 1]);
                spawnedWolfs.Remove(spawnedWolfs[spawnedWolfs.Count - 1]);
            }
            else
                spawnedWolfs.Remove(wolf);
        }
        public bool hasWolfAlive()
        { // return if there's still wolfs alive
            return spawnedWolfs.Count > 0;
        }
        private void Spawn(int wolf_classic, int wolf_water, int wolf_ice, bool spawnboss)
        {

            while (wolf_classic > 0)
            {
                //Spawn de loups par paquet
                int hounds = Random.Range(1, 5);
                if (hounds > wolf_classic)
                    hounds = wolf_classic;

                // Spawn random pour les loups classique et le boss.
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);

                for (int j = 0; j < hounds; j++)
                {
                    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                    spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                }
                // A partir du cycle 6, spawn loups montagne ou classique aleatoirement

                wolf_classic -= hounds;
            }


            int spawnlacIndex = Random.Range(0, spawnLac.Length);

            for (int j = 0; j < wolf_water; j++)
            {
                // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                spawnedWolfs.Add(Instantiate(enemy_water, spawnLac[spawnlacIndex].position, spawnLac[spawnlacIndex].rotation));
            }

            int spawnMountainIndex = Random.Range(0, spawnMountain.Length);

            for (int j = 0; j < wolf_ice; j++)
            {
                // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                spawnedWolfs.Add(Instantiate(enemy_ice, spawnMountain[spawnMountainIndex].position, spawnMountain[spawnMountainIndex].rotation));
            }

            if (spawnboss)
            {
                int spawnPointIndex = Random.Range(0, spawnPoints.Length);
                spawnedWolfs.Add(Instantiate(boss, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
            }
        }
    }
}