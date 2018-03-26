using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SO;
using UnityEngine.UI;

namespace SO.UI
{
    public class UpdateText : UIPropertyUpdater
    {
        public StringVariable targetString;
        public IntVariable targetInt;
        public Text targetText;
        
        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            if (targetText == null)
                targetText = GetComponent<Text>();

            if (targetString != null)
                targetText.text = targetString.value;
            else
                targetText.text = targetInt.value.ToString();
        }
        
        public void Raise(string target)
        {
            targetText.text = target;
        }
    }
}
