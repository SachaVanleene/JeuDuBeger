using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnVisibleInRangeEvent : MonoBehaviour {

    public SO.BoolVariable isVisibleAndInRange;
    public float range;
    public Transform compareTransform;

    public GameObject gameObjectRenderer;
    private MeshRenderer _renderer;
    private SO.ConditionalEvent_BoolVariable conditionalBoolEvent;

    private bool currentlyVisible;
    private bool previouslyVisible;

    private void Start()
    {
        isVisibleAndInRange.Set(false);
        currentlyVisible = false;
        previouslyVisible = false;
        _renderer = gameObjectRenderer.GetComponent<MeshRenderer>();
        conditionalBoolEvent = GetComponent<SO.ConditionalEvent_BoolVariable>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, compareTransform.position) < range)
        {
            currentlyVisible = _renderer.isVisible;
            if (!previouslyVisible && currentlyVisible)
            {
                isVisibleAndInRange.Set(true);
                conditionalBoolEvent.Raise();
            }
            else if (previouslyVisible && !currentlyVisible)
            {
                isVisibleAndInRange.Set(false);
                conditionalBoolEvent.Raise();
            }
            previouslyVisible = currentlyVisible;
        }
    }
}
