using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO.UI
{
    public class FadeAwayPanelDispatcher : UIPropertyUpdater {
        public GameObject panelPrefab;
        public Transform targetTransform;
        public float fadeDuration;
        public int panelsCount;

        public SO.IntVariable goldEarned;

        private Queue<int> panelsQueue;

	    // Use this for initialization
	    void Start ()
        {
            for (int i = 0; i < panelsCount; i++)
            {
                GameObject panel = Instantiate(panelPrefab, transform);
                panel.transform.SetParent(transform);
                panel.GetComponent<FadeAwayPanel>().Set(fadeDuration, targetTransform, CheckQueue);
                panel.SetActive(false);
            }

            panelsQueue = new Queue<int>();
        }

        public override void Raise()
        {
            int i = 0;
            while (i < transform.childCount && transform.GetChild(i).gameObject.activeInHierarchy)
                i++;

            Debug.Log(i);

            if (i < transform.childCount)
            {
                StartCoroutine(transform.
                    GetChild(i).
                    gameObject.
                    GetComponent<FadeAwayPanel>().
                    FadePanel());
            }
            else
            {
                panelsQueue.Enqueue(goldEarned.value);
            }
        }

        public void CheckQueue()
        {
            if (panelsQueue.Count > 0)
            {
                goldEarned.Set(panelsQueue.Dequeue());
                Raise();
            }    
        }
    }
}
