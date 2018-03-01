
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Script.Traps;
using Assets.Scripts.Traps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Script
{
    public class PanelTest : MonoBehaviour
    {
        private List<Button> _buttons = new List<Button>();

        // Use this for initialization
        void Start()
        {
            foreach (var child in GetComponentsInChildren<Button>())
            {
                Debug.Log("a");
                var child1 = child;
                child.onClick.AddListener(() =>
                {
                    SelectTrapType(child1);
                });
                _buttons.Add(child);
            }
        }

        public void SelectTrapType(Button targetButton)
        {
            foreach (var button in _buttons)
            {
                button.GetComponentInChildren<Text>().color = Color.black;
            }
            targetButton.GetComponentInChildren<Text>().color = Color.red;
            TrapFactory.SelectedTrapType = (TrapTypes) Enum.Parse(typeof(TrapTypes),targetButton.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
