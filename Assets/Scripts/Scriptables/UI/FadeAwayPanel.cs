using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SO.UI
{
    public class FadeAwayPanel : MonoBehaviour {

        public Text targetText;

        private float fadeDuration;
        private System.Action callback;

        private Color textColor;
        private Color negativeTextColor;
       
        public void Set(float fadeDuration, System.Action callback, Color textColor, Color negativeTextColor)
        {
            this.fadeDuration = fadeDuration;
            this.callback = callback;
            this.textColor = textColor;
            this.negativeTextColor = negativeTextColor;
        }


        public IEnumerator FadePanel(Transform targetTransform, bool isNegative)
        {
            if (isNegative)
                targetText.color = negativeTextColor;
            else
                targetText.color = textColor;

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

