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
	private int number_wolf;
	private int i = 0;
	private int temp;

	public void Begin_Night()
    {
        // + 5 loups toute les vague
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
        while (i < number_wolf)
        {
            // Call the Spawn function after a delay of the spawnTime
            temp = Spawn(i, Cycle);
            i = i + temp;
        }
    }
    public void WolfDeath(GameObject wolf = null)
    {
        number_wolf--;
        //spawnedWolfs.Remove(wolf);
    }
    public bool hasWolfAlive()
    { // return if there's still wolfs alive
        //return spawnedWolfs.Count > 0;
        return number_wolf != 0;
    }

    int Spawn (int total, int cycle)
	{
        //Spawn de loups par paquet
		int hounds = Random.Range(1, 5);
        while (hounds + total > number_wolf){
			hounds = Random.Range (1, 5);
		}

		// Spawn random pour les loups classique et le boss.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        //Si cycle <= 5, on ne spawn que des loups classique
        if(cycle <= 5)
        {
                for (int j = 0; j < hounds; j++)
                {
                    // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
                    spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
                }
        }

        // A partir du cycle 6, spawn loups montagne ou classique aleatoirement
        if(cycle > 5 && cycle <= 10)
        {
            int type = Random.Range(1, 3);

            //2 chances sur 3 de loups classique
            if(type <= 2)
            {
                spawnedWolfs.Add(Instantiate(enemy_classic, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
            }
            else //1 chance sur 3 spawn loup montagne
            {
                spawnedWolfs.Add(Instantiate(enemy_ice, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation));
            }
        }

        // A partir du cycle 11 spawn loup classique ou montagne ou lac
        if(cycle > 10)
        {
            int type = Random.Range(1, 6);

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
		return hounds;
	}
}
