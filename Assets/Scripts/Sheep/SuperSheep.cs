using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Managers;
using Assets.Script;
//using Assets.Scripts.Enclosures;

public class SuperSheep : MonoBehaviour
{
    public GameObject PinkSuperSheepPrefab;
    private bool _isDisplayingPanel = false;
    private int nbSuperSheep;

    public Material pinkFence;

    private GameManager _gameManager;


    public void AddPinkSuperSheep()
    {
        var sheep = Instantiate(PinkSuperSheepPrefab, this.transform); //crée un clone mouton
        sheep.transform.position = this.transform.position; //le place dans l'enclos
        sheep.transform.Rotate(0, Random.Range(0, 360), 0);

        foreach (Transform child in transform)
        {
            if(child.tag == "Fences")
            {
              Renderer r = child.GetComponent<Renderer>();
                r.material = pinkFence;
            }

        }
    }

    private void Awake()
    {
        nbSuperSheep = 0;
    }

    void Start()
    {
        _gameManager = GameManager.instance;
    }


    private void Update()
    {
        ShowPanel();
    }

    private void ShowPanel()
    {
        Vector3 distPlayertoEnclos = TerrainTest.PlayerGameObject.transform.position - transform.position;

        if (distPlayertoEnclos.magnitude < 25 && _gameManager.IsTheSunAwakeAndTheBirdAreSinging)
        {
            if (!EnclosureManager.EnclosurePannel.activeSelf)
            {
                EnclosureManager.EnclosurePannel.SetActive(true);
                _isDisplayingPanel = true;
            }
            HandleInputs();
        } else
        {
            if (EnclosureManager.EnclosurePannel.activeSelf && _isDisplayingPanel)
            {
                _isDisplayingPanel = false;
                EnclosureManager.EnclosurePannel.SetActive(false);
            }
        }
    }
    private void HandleInputs()
    {
        //appuie touche astérisk
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            if (nbSuperSheep < 1 && _gameManager.TotalSuperSheeps == 1)
            {
                AddPinkSuperSheep();
                nbSuperSheep++;
              //  this.GetComponent<EnclosureScript>().setHealth(50);
                _gameManager.PlaceSuperSheep();
            } 
        }
        
    }
}
