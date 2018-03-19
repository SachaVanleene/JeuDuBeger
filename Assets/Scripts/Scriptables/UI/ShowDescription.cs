//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ShowDescription : MonoBehaviour
{
    public GameObject descriptionGO;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    List<RaycastResult> results;

    public bool gameOver;

    public float idleDelay = .5f;
    float timer;
    public bool show;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);

        //Create a list of Raycast Results
        results = new List<RaycastResult>();

        timer = idleDelay;
        show = false;
        descriptionGO.gameObject.SetActive(false);
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetAxis("Mouse X") < 0.01 && Input.GetAxis("Mouse Y") < 0.01)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = idleDelay;
                show = false;
            }

            if (timer < 0 && !show)
            {
                show = true;

                //Set the Pointer Event Position to that of the mouse position
                m_PointerEventData.position = Input.mousePosition;

                //Raycast using the Graphics Raycaster and mouse click position
                m_Raycaster.Raycast(m_PointerEventData, results);
            }
        }
    }

    private void OnGUI()
    {
        if (show)
        {
            if (!descriptionGO.gameObject.activeInHierarchy && results.Count > 0)
            {
                //FRAGILE AS FUCK BOIII
                descriptionGO.transform.position = m_PointerEventData.position + new Vector2(12, -20f);

                string s = results[results.Count - 1].gameObject.transform.parent.name;
                string value;

                if (Strings.Description.TryGetValue(s, out value))
                    descriptionGO.GetComponentInChildren<Text>().text = value;

                descriptionGO.gameObject.SetActive(true);
            }
        }
        else
        {
            descriptionGO.gameObject.SetActive(false);
        }
    }
}