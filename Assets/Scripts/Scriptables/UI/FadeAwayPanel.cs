using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class FadeAwayPanel : UIPropertyUpdater {

        public Transform targetTransform;
        public float fadeDuration;

        public Text targetText;
        public bool isFading;

        private float timer;

        private Vector3 initialPosition;
        private Vector3 targetPosition;

        public override void Raise()
        {
            if (!isFading)
            {
                targetText.CrossFadeAlpha(0.0f, fadeDuration, true);
                StartCoroutine(IsFading());
            }
        }

        IEnumerator IsFading()
        {
            isFading = true;
            timer = 0;
            initialPosition = gameObject.transform.position;
            targetPosition = targetTransform.position;

            gameObject.SetActive(true);
            while (timer < fadeDuration)
            { 
                gameObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, timer / fadeDuration);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
            gameObject.transform.position = initialPosition;
            isFading = false;
            targetText.CrossFadeAlpha(1.0f, 0f, true);
            
        }
    }
}

