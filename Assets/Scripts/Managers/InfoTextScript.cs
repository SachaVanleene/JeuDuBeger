using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextScript : MonoBehaviour
{
    public GameObject Parent;

    private Text text;
    private int duration = 2;

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
    }
    void Start()
    {     
        text.text = "";
        Parent.SetActive(false);
    }
    public void Clear()
    {
        text.text = "";
        Parent.SetActive(false);
        StopAllCoroutines();
    }
    public void DisplayInfo(string msg, int duration)
    {
        Parent.SetActive(true);

        text.text = msg;
        this.duration = duration;
        StopAllCoroutines();
        StartCoroutine(clearMessage());
    }
    private IEnumerator clearMessage()
    {
        yield return new WaitForSeconds(duration);
        Clear();
    }
}
