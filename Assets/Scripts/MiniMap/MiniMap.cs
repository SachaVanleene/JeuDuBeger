﻿using System;
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

    private GameObject _player;
    private List<Text> _sheepNumberList = new List<Text>();
    private float deplacementFactor = 300f;
    private float deplacementPadding = 150f;
 
    void Start ()
	{
        _player = GameObject.FindGameObjectWithTag("Player");
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
                text = Instantiate(SheepNumberPrefab,transform);

                text.transform.localPosition = new Vector2(normalizedPos.x * deplacementFactor - deplacementPadding, 
                    normalizedPos.y * deplacementFactor - deplacementPadding);
                text.GetComponent<Text>().text = enclosure.SheepNumber.ToString();
                _sheepNumberList.Add(text);
            }
        }
    }
	public void UpdateEnclosure(int enclosureOrder)
	{
	    _sheepNumberList[enclosureOrder].GetComponent<Text>().text =
	        EnclosureManager.EnclosureList[enclosureOrder].SheepNumber.ToString();
	}
	void Update () {
	    var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.activeTerrain.terrainData.size.x, _player.transform.position.x),
	        Mathf.InverseLerp(0, Terrain.activeTerrain.terrainData.size.z, _player.transform.position.z));
	    Farmer.transform.localPosition = new Vector2(normalizedPos.x * deplacementFactor - deplacementPadding, 
            normalizedPos.y * deplacementFactor - deplacementPadding);

    }
}
