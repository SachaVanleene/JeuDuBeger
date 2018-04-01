using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEvent : MonoBehaviour {

    //Raise ConditionalEvent on trigger Enter/Exit
    public SO.BoolVariable inRange;
    public string compareTag = "Player";

    private void Start()
    {
        inRange.Set(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            Debug.Log("salut");
            inRange.Set(true);
            GetComponent<SO.ConditionalEvent_BoolVariable>().Raise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            Debug.Log("bye");
            inRange.Set(false);
            GetComponent<SO.ConditionalEvent_BoolVariable>().Raise();
        }
    }
}
