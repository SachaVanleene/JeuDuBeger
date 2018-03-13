using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Managers;
//using Assets.Scripts.Enclosures;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class EnclosureManager : MonoBehaviour
{

    public List<Vector3> EnclosurePositionList;
    public List<EnclosureScript> EnclosPrefabList;
    public GameObject House;

    public static EnclosureManager Instance = null;
    public static Vector3 HousePosition;
    public static GameObject EnclosurePannel;
    public static int SheepNumberInTheWorld;

    public  static List<EnclosureScript> EnclosureList = new List<EnclosureScript>();
    private GameManager _gameManager;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start () {
        EnclosurePannel = GameObject.FindWithTag("EnclosurePannel");
        EnclosurePannel.SetActive(false);

        _gameManager = GameManager.instance;

        HousePosition = House.transform.position;

	    foreach (var enclosurePosition in EnclosurePositionList)
	    {
	        EnclosureScript enclosure;
            float distance = Vector3.Distance(House.transform.position, enclosurePosition);
	        if (distance < 70)
	        {
	            enclosure = Instantiate(EnclosPrefabList[0]);
	            enclosure.transform.position = enclosurePosition;
	        }
            else if (distance < 120)
	        {
	            enclosure = Instantiate(EnclosPrefabList[1]);
	            enclosure.transform.position = enclosurePosition;
            }
            else
	        {
	            enclosure = Instantiate(EnclosPrefabList[2]);
	            enclosure.transform.position = enclosurePosition;
            }
	        EnclosureList.Add(enclosure);
        }
        EnclosureList = EnclosureList.OrderBy(o=>o.Distance).ToList();
    }
    public void DefaultFilling()
    {
        foreach (var enclosure in EnclosureList)
        {
            while (_gameManager.TotalSheeps > 0)
            {
                enclosure.Health += 10;
                if (enclosure.Health >= 100)
                    break;
            }
        }
    }

    public void TakeOffAllSheeps()
    {
        _gameManager.TotalSheeps = SheepNumberInTheWorld;
        EnclosurePannel.transform.GetChild(1).GetComponent<Text>().text ="0";
        foreach (var enclosure in EnclosureList)
        {
            enclosure.KillAllSheep();
        }
    }
}
