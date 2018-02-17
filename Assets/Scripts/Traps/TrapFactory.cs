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
        public static TrapTypes SelectedTrapType = TrapTypes.NeedleTrap;
        public static Boolean IsInTrapCreationMode = false;
        public static GameObject ActualTrap;
        public static int ActionRange = 15;

        public static Vector3 GetMousePosition()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            float distance = Vector3.Distance(hitInfo.point, TerrainTest.PlayerGameObject.transform.position);
            if (distance <= ActionRange && hitInfo.point != Vector3.zero)
            {
                return hitInfo.point;
            }
            if (hitInfo.point != Vector3.zero)
            {
                Vector3 positionAtRange = ray.direction * ActionRange + TerrainTest.PlayerGameObject.transform.position;
                return  new Vector3(positionAtRange.x, TerrainTest.Terrain.SampleHeight(positionAtRange),positionAtRange.z);
            }
            else
            {

                Vector3 positionAtRange = ray.direction * ActionRange + TerrainTest.PlayerGameObject.transform.position;
                return new Vector3(positionAtRange.x, TerrainTest.Terrain.SampleHeight(positionAtRange), positionAtRange.z);
            }
              
        }


    }
}
