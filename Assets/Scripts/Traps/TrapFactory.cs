using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using Assets.Script.Traps;
using UnityEngine;

namespace Assets.Script.Factory
{
    static class TrapFactory 
    {
        public static TrapTypes SelectedTrapType = TrapTypes.MudTrap;
        public static Boolean IsInTrapCreationMode = true;
        public static GameObject ActualTrap;

        public static Vector3 GetMousePosition()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            return hitInfo.point;
        }


    }
}
