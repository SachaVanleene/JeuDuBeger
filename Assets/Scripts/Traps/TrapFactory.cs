using System;
using Assets.Script;
using Assets.Script.Managers;
using Assets.Script.Traps;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Traps
{
   /** static class TrapFactory
    {
        public static TrapTypes SelectedTrapType = TrapTypes.NeedleTrap;
        public static Boolean IsInTrapCreationMode = false;
        private static Trap _closestTrap;

        public static Trap ClosestTrap
        {
            get { return _closestTrap; }
            set
            {
                _closestTrap = value;
                TerrainTest.TrapLevelUpPannel.gameObject.SetActive(false);
                if (_closestTrap == null) return;
                TerrainTest.TrapLevelUpPannel.AdjustPosition();
                TerrainTest.TrapLevelUpPannel.gameObject.SetActive(true);
            }
        }

        public static Trap ActualTrap;
        public static int ActionRange = 15;

        public static Boolean IsColliding()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f));
            Physics.Raycast(ray, out hitInfo, 100);
            float distance = Vector3.Distance(hitInfo.point, TerrainTest.PlayerGameObject.transform.position);
            if (distance <= ActionRange && hitInfo.collider.tag == "Trap" && GameManager.instance.IsTheSunAwakeAndTheBirdAreSinging)
            {
                ClosestTrap = hitInfo.collider.gameObject.GetComponentInChildren<Trap>();
                ClosestTrap.Select();
                return true;
            }
            if(ClosestTrap != null)
                ClosestTrap.Deselect();
            ClosestTrap = null;
            return false;
        }


        public static Vector3 GetMousePosition()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f));
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            float distance = Vector3.Distance(hitInfo.point, TerrainTest.PlayerGameObject.transform.position);

            if (distance <= ActionRange && hitInfo.point != Vector3.zero && hitInfo.collider.gameObject.tag != "Enclos")
            {
                return hitInfo.point;
            }
            if (hitInfo.point != Vector3.zero)
            {
                Vector3 positionAtRange = ray.direction * ActionRange + TerrainTest.PlayerGameObject.transform.position;
                return new Vector3(positionAtRange.x, TerrainTest.Terrain.SampleHeight(positionAtRange), positionAtRange.z);
            }
            else
            {
                Vector3 positionAtRange = ray.direction * ActionRange + TerrainTest.PlayerGameObject.transform.position;
                return new Vector3(positionAtRange.x, TerrainTest.Terrain.SampleHeight(positionAtRange), positionAtRange.z);
            }
        }


    }**/
}
