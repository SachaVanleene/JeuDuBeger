using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextScript : MonoBehaviour
{

    private Text text;
    private int duration = 2;
    void Start()
    {
        text = gameObject.GetComponent<Text>();
        text.text = "";
    }
    public void DisplayInfo(string msg, int duration)
    {
        text.text = msg;
        this.duration = duration;
        StartCoroutine(clearMessage());
    }
    private IEnumerator clearMessage()
    {
        yield return new WaitForSeconds(duration);
        text.text = "";
    }
}
