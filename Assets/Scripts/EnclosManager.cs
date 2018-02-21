using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnclosManager : MonoBehaviour {

    public GameObject sheep, enclos, panelEnclos;
    public Text totalSheep;
    public ParticleSystem smoke;
    public float RewardGold = 1.0f;

    private int nbSheep = -1;
    public int NbSheep
    {
        get
        {
            return this.nbSheep;
        }
        set
        {
            this.nbSheep = value;
        }
    }
    
    private float health;
    private bool activePanel;
    private GameObject[] sheepClone = new GameObject[10];

    private Assets.Script.Managers.GameManager _gameManager;

    void Start () {
        nbSheep = -1;
        health = 0.0f;
        activePanel = false;

        _gameManager = Assets.Script.Managers.GameManager.instance;
    }
	
	void Update () {
        affichePanel();

        if(activePanel) {
            panelEnclos.gameObject.SetActive(true);
            handleInputs();
        }
        else {
            panelEnclos.gameObject.SetActive(false);
        }
    }

    //******************************************************************
    //Affichage du panel de l'enclos
    //******************************************************************
    private void affichePanel() {
        Vector3 distPlayertoEnclos = GameObject.FindWithTag("Player").transform.position - enclos.transform.position;

        if(distPlayertoEnclos.magnitude < 25) activePanel = true;
        else activePanel = false;
    }

    //******************************************************************
    //Handle Inputs "+" et "-" le nombre de moutons
    //******************************************************************
    private void handleInputs() {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            AddSheep();

            //Vie de l'enclos
            if (nbSheep == 9) health = 100; //santé max
            else health += 10;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            RemoveSheep();

            //Vie de l'enclos
            if (nbSheep == -1) health = 0; //santé min
            else health -= 10;
        }
    }

    //******************************************************************
    //Ajouter mouton
    //******************************************************************
    public void AddSheep() {
        if (_gameManager.TotalSheeps <= 0)
            return;
        if (nbSheep < 9) {
            nbSheep++;
            totalSheep.text = (nbSheep + 1).ToString();

            sheepClone[nbSheep] = Instantiate(sheep, enclos.transform); //crée un clone mouton
            sheepClone[nbSheep].transform.rotation = enclos.transform.rotation; //le place dans l'enclos
            sheepClone[nbSheep].transform.Rotate(0, Random.Range(0, 360), 0); //l'oriente d'une façon aléatoire

            _gameManager.PlaceSheep();
        }
    }

    //******************************************************************
    //Supprimer mouton
    //******************************************************************
    public void RemoveSheep() {
        if (nbSheep >= 0) {
            Instantiate(smoke, sheepClone[nbSheep].transform.position, sheepClone[nbSheep].transform.rotation);
            Destroy(sheepClone[nbSheep], .5f); //suppression du dernier clone créé

            totalSheep.text = nbSheep.ToString();
            nbSheep--;

            _gameManager.TakeSheep();
        }
    }

    //******************************************************************
    //Prise de dégats de l'enclos
    //******************************************************************
    public void DamageEnclos(float degats) {
        health -= degats;
        if (health < 0) health = 0; //santé min

        int diffSheep = (nbSheep + 1) - Mathf.RoundToInt(health / 10.0f);

        for (int i=0; i < diffSheep; i++) {
            RemoveSheep();
        }
    }
}