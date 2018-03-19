using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script;
using Assets.Script.Managers;
using Assets.Scripts.Enclosures;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Text SheepNumberPrefab;
    public List<EnclosureScript> EnclosureList;
    public Image Farmer;

    private List<Text> _sheepNumberList = new List<Text>();
	// Use this for initialization
	void Start ()
	{       
	}

    public void InstantiateText()
    {
        Text text;
        foreach (var enclosure in EnclosureManager.EnclosureList)
        {
            if (enclosure.SheepNumber == 0)
            {
                var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.activeTerrain.terrainData.size.x, enclosure.transform.position.x),
                    Mathf.InverseLerp(0, Terrain.activeTerrain.terrainData.size.z, enclosure.transform.position.z));
                text = Instantiate(SheepNumberPrefab);
                text.transform.parent = this.transform;
                text.transform.localPosition = new Vector2(normalizedPos.x * 100 - 45, normalizedPos.y * 100 - 50);
                text.GetComponent<Text>().text = enclosure.SheepNumber.ToString();
                _sheepNumberList.Add(text);
            }
        }
        Debug.Log(_sheepNumberList);
    }
	public void UpdateEnclosure(int enclosureOrder)
	{
	    _sheepNumberList[enclosureOrder].GetComponent<Text>().text =
	        EnclosureManager.EnclosureList[enclosureOrder].SheepNumber.ToString();
	}
	// Update is called once per frame
	void Update () {
	    var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.activeTerrain.terrainData.size.x, TerrainTest.PlayerGameObject.transform.position.x),
	        Mathf.InverseLerp(0, Terrain.activeTerrain.terrainData.size.z, TerrainTest.PlayerGameObject.transform.position.z));
	    Farmer.transform.localPosition = new Vector2(normalizedPos.x * 100 - 50, normalizedPos.y * 100 - 50);

    }
}
