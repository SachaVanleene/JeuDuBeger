using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWolves : MonoBehaviour {

    public GameObject common_wolf;
    public GameObject water_wolf;
    public GameObject mountain_wolf;
    public GameObject boss_wolf;


    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 spawning_position = this.transform.position + 10 * this.transform.forward;
        if (Input.GetKey(KeyCode.Keypad1))
        {
            Instantiate(common_wolf, spawning_position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            Instantiate(water_wolf, spawning_position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            Instantiate(mountain_wolf, spawning_position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            Instantiate(boss_wolf, spawning_position, Quaternion.identity);
        }
    }
}
