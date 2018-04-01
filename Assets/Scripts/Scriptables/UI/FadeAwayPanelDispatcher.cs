using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class FadeAwayPanelDispatcher : UIPropertyUpdater {

        [Header("Main variables")]
        public GameObject panelPrefab;
        public Transform targetTransform;
        public float fadeDuration;
        public int panelsCount;
        public SO.StringVariable targetString;
        public Color textColor;

        [Space]
        [Header("Extra : positive & negative feedback")]
        public bool enableExtra;
        public string negativeIfStringContains;
        public Transform negativeTargetTransform;
        public Color negativeTextColor;

        private Queue<string> panelsQueue;
        private bool isNegative;

	    // Use this for initialization
	    void Start ()
        {
            for (int i = 0; i < panelsCount; i++)
            {
                GameObject panel = Instantiate(panelPrefab, transform);
                panel.transform.SetParent(transform);
                panel.GetComponent<FadeAwayPanel>().Set(fadeDuration, CheckQueue, textColor, negativeTextColor);
                panel.GetComponent<UpdateText>().targetString = targetString;
                panel.transform.GetChild(0).gameObject.GetComponent<Text>().color = textColor;
                panel.SetActive(false);
            }

            panelsQueue = new Queue<string>();
        }

        public override void Raise()
        {
            int i = transform.childCount;
            while (i > transform.childCount - panelsCount && transform.GetChild(i).gameObject.activeInHierarchy)
                i--;

            Transform currentTargetTransform = targetTransform;
            if (i > transform.childCount - panelsCount)
            {
                if (enableExtra && !targetString.value.Contains(negativeIfStringContains))
                {
                    if (negativeTargetTransform != null)
                        currentTargetTransform = negativeTargetTransform;
                    isNegative = true;
                }
                else
                {
                    isNegative = false;
                }

                StartCoroutine(transform.
                        GetChild(i).
                        gameObject.
                        GetComponent<FadeAwayPanel>().
                        FadePanel(currentTargetTransform, isNegative));
            }
            else
            {
                panelsQueue.Enqueue(targetString.value);
            }
        }

        public void CheckQueue()
        {
            if (panelsQueue.Count > 0)
            {
                targetString.Set(panelsQueue.Dequeue());
                Raise();
            }    
        }
    }
}
