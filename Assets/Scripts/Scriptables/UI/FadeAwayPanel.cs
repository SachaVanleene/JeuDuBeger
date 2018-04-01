using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class FadeAwayPanel : MonoBehaviour {

        public Text targetText;

        private float fadeDuration;
        private Transform targetTransform;
        private System.Action callback;

        public void Set(float fadeDuration, Transform targetTransform, System.Action callback)
        {
            this.fadeDuration = fadeDuration;
            this.targetTransform = targetTransform;
            this.callback = callback;
        }


        public IEnumerator FadePanel()
        {
            float timer = 0;
            Vector3 initialPosition = gameObject.transform.position;
            Vector3 targetPosition = targetTransform.position;

            gameObject.SetActive(true);
            targetText.CrossFadeAlpha(0.0f, fadeDuration, true);

            while (timer < fadeDuration)
            { 
                gameObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, timer / fadeDuration);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
            gameObject.transform.position = initialPosition;
            targetText.CrossFadeAlpha(1.0f, 0f, true);
            
            callback();
        }
    }
}

