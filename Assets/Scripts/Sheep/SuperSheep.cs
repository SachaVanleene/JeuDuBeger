using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Managers;
using Assets.Script;
//using Assets.Scripts.Enclosures;
[System.Obsolete]
public class SuperSheep : MonoBehaviour
{
    public GameObject PinkSuperSheepPrefab;
    public GameObject BlackSuperSheepPrefab;
    private bool _isDisplayingPanel = false;
    private int nbSuperSheepPink;
    private int nbSuperSheepBlack;
    public Material pinkFence;
    public Material blackFence;
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

    public void AddBlackSuperSheep()
    {
        var sheep = Instantiate(BlackSuperSheepPrefab, this.transform); //crée un clone mouton
        sheep.transform.position = this.transform.position; //le place dans l'enclos
        sheep.transform.Rotate(0, Random.Range(0, 360), 0);

        foreach (Transform child in transform)
        {
            if (child.tag == "Fences")
            {
                Renderer r = child.GetComponent<Renderer>();
                r.material = blackFence;
            }

        }
    }

    private void Awake()
    {
        nbSuperSheepPink = 0;
        nbSuperSheepBlack = 0;
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
       /** Vector3 distPlayertoEnclos = TerrainTest.PlayerGameObject.transform.position - transform.position;

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
        }**/
    }
    private void HandleInputs()
    {
        //appuie touche astérisk
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            if (nbSuperSheepPink < 1 && _gameManager.TotalSuperSheepsPink == 1)
            {
                AddPinkSuperSheep();
                nbSuperSheepPink++;
              //  this.GetComponent<EnclosureScript>().setHealth(50);
                _gameManager.PlaceSuperSheep();
            } 
        }
        if (Input.GetKeyDown(KeyCode.KeypadDivide))
        {
            if (nbSuperSheepBlack < 1 && _gameManager.TotalSuperSheepsPink == 1)
            {
                AddBlackSuperSheep();
                nbSuperSheepBlack++;
                //  this.GetComponent<EnclosureScript>().setHealth(50);
                _gameManager.PlaceSuperSheep();
            }
        }

    }
}
