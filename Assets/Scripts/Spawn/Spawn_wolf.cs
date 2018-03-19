using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_wolf : MonoBehaviour {

	public GameObject enemy_classic;        // Loups Normaux
    public GameObject enemy_water;          // Loups lacs
    public GameObject enemy_ice;            // Loups montagne
    public Transform[] spawnPoints;         // Spawn classique
    public GameObject spawnLac;             // Spawn lac
    public GameObject spawnMountain;        // Spawn montagne
    public int Cycle { get; set; }
    private List<GameObject> spawnedWolfs = new List<GameObject>();

    public void Begin_Night()
    {
        int number_wolf;
        // + 5 loups à chaque vagues
        if (Cycle <= 20)
        {
            number_wolf = Cycle * 5;
        }

        //Quand 100 loup spawn, augmenter leur vie a chaque cycle
        else
        {
            number_wolf = 100;
            //wolf_health += 10;
            //wolf_lac_healt += 10
            //wolf_lac_healt += 10
        }
        Spawn(number_wolf);
        /*
        int i = 0;
        int temp;
        while (i < number_wolf)
        {
            // Call the Spawn function after a delay of the spawnTime
            temp = Spawn(i, Cycle);
            i = i + temp;
        }*/
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
    private void Spawn (int total)
	{
        while (total > 0)
        {
            //Spawn de loups par paquet
            int hounds = Random.Range(1, 5);
            if (hounds > total)
                hounds = total;

            // Spawn random pour les loups classique et le boss.
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            //Si cycle <= 5, on ne spawn que des loups classique
            if (Cycle <= 5)
            {
                for (int j = 0; j < hounds; j++)
                {
                    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                    spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                }
            }
            // A partir du cycle 6, spawn loups montagne ou classique aleatoirement
            else if (Cycle <= 10)
            {
                int type = Random.Range(1, 3);
                for (int j = 0; j < hounds; j++)
                {
                    //2 chances sur 3 de loups classique
                    if (type <= 2)
                    {
                        spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                    }
                    else //1 chance sur 3 spawn loup montagne
                    {
                        spawnedWolfs.Add(Instantiate(enemy_ice, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                    }
                }
            }
            // A partir du cycle 11 spawn loup classique ou montagne ou lac
            else if (Cycle > 10)
            {
                int type = Random.Range(1, 6);
                for (int j = 0; j < hounds; j++)
                {
                    //4 chances sur 6 de loups classique
                    if (type <= 4)
                    {
                        spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                    }
                    else if (type == 5)//1 chance sur 6 spawn loup montagne
                    {
                        spawnedWolfs.Add(Instantiate(enemy_ice, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                    }
                    else if (type == 6)//1 chance sur 6 spawn loup lac
                    {
                        spawnedWolfs.Add(Instantiate(enemy_water, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                    }
                }
            }
            total -= hounds;
        }
    }
}
