using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Managers;
using Assets.Script.Traps;
using Assets.Scripts.Traps;
using UnityEngine;

namespace Assets.Script
{
    public class TerrainTest : MonoBehaviour
    {
        public List<Trap> Traps;
        public TrapTypes ActualSelectedTrapTypes;
        public static Terrain Terrain;
        public static GameObject PlayerGameObject;
        public static GameObject MainCanvasGameObject;
        private GameManager _gameManager;



        public void Start()
        {
            PlayerGameObject = GameObject.FindWithTag("Player");
            _gameManager = GameManager.instance;
            MainCanvasGameObject  = GameObject.FindWithTag("MainCanvas");
            Terrain = Terrain.activeTerrain;
            ActualSelectedTrapTypes = TrapTypes.None;
        }

        public void Update()
        {
            if (_gameManager.IsTheSunAwakeAndTheBirdAreSinging)
            {
                if (Input.GetKey("1"))
                {
                    TrapFactory.IsInTrapCreationMode = true;
                    TrapFactory.SelectedTrapType = TrapTypes.NeedleTrap;
                }
                if (Input.GetKey("2"))
                {
                    TrapFactory.IsInTrapCreationMode = true;
                    TrapFactory.SelectedTrapType = TrapTypes.BaitTrap;
                }
                if (Input.GetKey("3"))
                {
                    TrapFactory.IsInTrapCreationMode = true;
                    TrapFactory.SelectedTrapType = TrapTypes.MudTrap;
                }
                if (Input.GetKey("4"))
                {
                    TrapFactory.IsInTrapCreationMode = true;
                    TrapFactory.SelectedTrapType = TrapTypes.LandmineTrap;
                }
            }
            else
            {
                TrapFactory.IsInTrapCreationMode = false;
                ActualSelectedTrapTypes = TrapTypes.None;
                Destroy(TrapFactory.ActualTrap);
            }
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
                if (Input.GetMouseButtonDown(0))
                    CreateTrap();
                if (Input.GetMouseButtonDown(1))
                {
                    TrapFactory.IsInTrapCreationMode = false;
                    ActualSelectedTrapTypes = TrapTypes.None;
                    Destroy(TrapFactory.ActualTrap);
                }
            }
        }

        public void CreateTrapPrevu()
        {
            Trap trap = Traps[(int) TrapFactory.SelectedTrapType];
            trap.IsInPreviewMode = true;
            GameObject trapGameObject = trap.TrapPrefab;
            trapGameObject.transform.position = TrapFactory.GetMousePosition();
            TrapFactory.ActualTrap = Instantiate(trapGameObject);
        }
        public void CreateTrap()
        {
            if (Math.Abs(TrapFactory.ActualTrap.GetComponentInChildren<Renderer>().material.color.r - 205) > 0.1)
            {
                Trap t = Traps[(int) TrapFactory.SelectedTrapType];
                GameObject trap = Instantiate(t.TrapPrefab);
                ((Trap) trap.GetComponentInChildren(typeof(Trap))).IsInPreviewMode = false;
                foreach (var rend in trap.GetComponentsInChildren<Renderer>())
                {
                    var newMaterial = new Material(rend.material);
                    newMaterial.color = Color.grey;
                    rend.material = newMaterial;
                }
                Vector3 mousePosition = TrapFactory.GetMousePosition();
                var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.terrainData.size.x, mousePosition.x),
                    Mathf.InverseLerp(0, Terrain.terrainData.size.z, mousePosition.z));
                trap.transform.rotation =
                    Quaternion.LookRotation(Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y),
                        Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
                trap.transform.position = TrapFactory.GetMousePosition();
            }
        }

    }
}
