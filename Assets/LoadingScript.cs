using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour {
    private float _nextActionTime = 0.0f;
    private float _period = 1f;

    private int _i = 1;
    // Use this for initialization
    void Start () {
		
	}
    void FixedUpdate()
    {
        if (Time.time > _nextActionTime)
        {
            _nextActionTime += _period;
            string str = "Chargement";
            for (int i = 0; _i % 3 >= i; i++)
                str += " .";
            this.GetComponent<Text>().text = str;
            _i++;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
