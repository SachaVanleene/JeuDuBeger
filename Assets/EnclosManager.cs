using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnclosManager : MonoBehaviour {

    public GameObject sheep, enclos, panelEnclos;
    public Text totalSheep;
    public ParticleSystem smoke;

    private int nbSheep;
    private GameObject[] sheepClone = new GameObject[10];

    // Use this for initialization
    void Start () {
        nbSheep = -1;
    }
	
	// Update is called once per frame
	void Update () {
        handleInputs();
    }

    //Input temp
    private void handleInputs() {
        if(Input.GetKeyDown(KeyCode.E)) panelEnclos.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.R)) panelEnclos.gameObject.SetActive(false);
    }

    //Ajouter mouton
    public void AddSheep() {
        if (nbSheep < 9) {
            nbSheep++;
            totalSheep.text = (nbSheep + 1).ToString();

            sheepClone[nbSheep] = Instantiate(sheep, enclos.transform); //crée un clone mouton
            sheepClone[nbSheep].transform.rotation = enclos.transform.rotation; //le place dans l'enclos
            sheepClone[nbSheep].transform.Rotate(0, Random.Range(0, 360), 0); //l'oriente d'une façon aléatoire
        }
    }

    //Supprimer mouton
    public void RemoveSheep() {
        if (nbSheep >= 0) {
            Instantiate(smoke, sheepClone[nbSheep].transform.position, sheepClone[nbSheep].transform.rotation);
            Destroy(sheepClone[nbSheep], .5f); //suppression du dernier clone créé

            totalSheep.text = nbSheep.ToString();
            nbSheep--;
        }
    }
}