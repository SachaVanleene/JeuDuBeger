using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Factory;
using Assets.Script.Traps;
using UnityEngine;

namespace Assets.Script
{
    public class TerrainTest : MonoBehaviour
    {
        public List<GameObject> Traps;
        public TrapTypes ActualSelectedTrapTypes;
        public Terrain Terrain;
        public void Start()
        {
            CreateTrapPrevu();
            ActualSelectedTrapTypes = TrapFactory.SelectedTrapType;
        }

        public void Update()
        {
            if (TrapFactory.IsInTrapCreationMode)
            {
                if (TrapFactory.SelectedTrapType != ActualSelectedTrapTypes)
                {
                    Destroy(TrapFactory.ActualTrap);
                    CreateTrapPrevu();
                    ActualSelectedTrapTypes = TrapFactory.SelectedTrapType;
                }
                Vector3 mousePosition = TrapFactory.GetMousePosition();
                var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.terrainData.size.x, mousePosition.x),
                    Mathf.InverseLerp(0, Terrain.terrainData.size.z, mousePosition.z));
                TrapFactory.ActualTrap.transform.rotation = Quaternion.LookRotation(Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y), Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
                TrapFactory.ActualTrap.transform.position = mousePosition;
                if (Input.GetMouseButtonDown(1))
                    CreateTrap();
            }
        }

        public void CreateTrapPrevu()
        {
            GameObject trap = Traps[(int)TrapFactory.SelectedTrapType];
            trap.transform.position = TrapFactory.GetMousePosition();
            TrapFactory.ActualTrap = Instantiate(trap);
            foreach (var rend in TrapFactory.ActualTrap.GetComponentsInChildren<Renderer>())
            {
                rend.material.color = new Color(10, 205, 0, 0.02f);
            }

        }
        public void CreateTrap()
        {
            GameObject trap = Instantiate(Traps[(int)TrapFactory.SelectedTrapType]);
            Vector3 mousePosition = TrapFactory.GetMousePosition();
            var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.terrainData.size.x,mousePosition.x),
                Mathf.InverseLerp(0, Terrain.terrainData.size.z, mousePosition.z));
            trap.transform.rotation = Quaternion.LookRotation(Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y), Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
            trap.transform.position = TrapFactory.GetMousePosition();
        }

    }
}
