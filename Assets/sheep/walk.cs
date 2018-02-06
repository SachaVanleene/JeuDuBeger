using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour {

    //Variables
    private Vector3 direction = new Vector3(0, 0, 0.002f);

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction.x, direction.y, direction.z);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Fences") || other.gameObject.CompareTag("Sheep")) {
            transform.Rotate(0, 180, 0);
            transform.Translate(direction.x+0.005f, direction.y, direction.z); //poussé pour éviter problème quand collé à la barrière
        }
    }
}