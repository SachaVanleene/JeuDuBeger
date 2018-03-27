using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health_Boss : MonoBehaviour {

    [Header("HealtBarReference")]
    public Image healtBar;
    public GameObject canevas;
    public float displayTime;
    public GameObject camera;

    WolfBossHealth health_script;
    int healt_max;
    bool isDisplaying;



    // Use this for initialization
    void Start()
    {
        health_script = GetComponent<WolfBossHealth>();
        healt_max = health_script.GetHealthMax();
        healtBar.fillAmount = 1;
        canevas.SetActive(false);
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    public void OnHit()
    {
        float value = (float)health_script.getHealth() / (float)healt_max;
        healtBar.fillAmount = value;
        isDisplaying = true;
        canevas.SetActive(true);
        StartCoroutine(WaitAndDisableHealthBar());
    }

    IEnumerator WaitAndDisableHealthBar()
    {
        isDisplaying = false;
        yield return new WaitForSeconds(displayTime);
        if (!isDisplaying)
        {
            canevas.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canevas.activeSelf)
        {
            canevas.transform.LookAt(canevas.transform.position + camera.transform.rotation * Vector3.forward,
             camera.transform.rotation * Vector3.up);
        }
    }
}
