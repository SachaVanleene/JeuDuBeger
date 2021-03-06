﻿using System;
using System.Collections.Generic;
using Assets.Script.Managers;
using Assets.Script.Traps;
using UnityEngine;

namespace Assets.Scripts.Traps
{
    public class TrapCreator : MonoBehaviour
    {
        public List<Trap> Traps;
        public TrapTypes ActualSelectedTrapTypes;

        public static Terrain Terrain;
        public static GameObject PlayerGameObject;
        public static GameObject MainCanvasGameObject;
        public static UiManager TrapLevelUpPannel;
        public static TrapTypes SelectedTrapType = TrapTypes.None;
        public static Boolean IsInTrapCreationMode = false;
        public static Trap ActualTrap;
        public static int ActionRange = 15;

        private static Trap _targetedTrap;
        public static Trap TargetedTrap
        {
            get { return _targetedTrap; }
            set
            {
                if (_targetedTrap != null)
                    _targetedTrap.Deselect();
                _targetedTrap = value;
                TrapLevelUpPannel.gameObject.SetActive(false);
                if (_targetedTrap == null) return;
                TrapLevelUpPannel.AdjustPosition();
                TrapLevelUpPannel.gameObject.SetActive(true);
            }
        }

        private GameManager _gameManager;

        public void Start()
        {
            TrapLevelUpPannel = FindObjectOfType<UiManager>();
            PlayerGameObject = GameObject.FindWithTag("Player");
            _gameManager = GameManager.instance;
            MainCanvasGameObject = GameObject.FindWithTag("MainCanvas");
            Terrain = Terrain.activeTerrain;
            ActualSelectedTrapTypes = TrapTypes.None;
        }

        public void Update()
        {
            if (_gameManager.IsTheSunAwakeAndTheBirdAreSinging)
            {
                if (Input.GetKey("1"))
                {
                    IsInTrapCreationMode = true;
                    UpdateUi(TrapTypes.NeedleTrap);
                }
                if (Input.GetKey("2"))
                {
                    IsInTrapCreationMode = true;
                    UpdateUi(TrapTypes.BaitTrap);
                }
                if (Input.GetKey("3"))
                {
                    IsInTrapCreationMode = true;
                    UpdateUi(TrapTypes.MudTrap);
                }
                if (Input.GetKey("4"))
                {
                    IsInTrapCreationMode = true;
                    UpdateUi(TrapTypes.LandmineTrap);
                }
            }
            else
            {
                IsInTrapCreationMode = false;
                ActualSelectedTrapTypes = TrapTypes.None;
                Debug.Log(TargetedTrap);
                TargetedTrap = null;
                Debug.Log(TargetedTrap);

                if (ActualTrap != null)
                    Destroy(ActualTrap.TrapPrefab);
            }
            if (IsInTrapCreationMode)
            {   //  hackfix : avoid allowing traps then put in pause
                TutorialOpen.instance.OpenTutorial(GameVariables.Tutorials.traps);
            }
            if (IsInTrapCreationMode)
            {
                TargetedTrap = null;
                if (SelectedTrapType != ActualSelectedTrapTypes)
                {
                    if(ActualTrap != null)
                        Destroy(ActualTrap.TrapPrefab);
                    CreateTrapPrevu();
                    ActualSelectedTrapTypes = SelectedTrapType;
                }
                Vector3 cursorPosition = GetCursorPosition();
                var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.terrainData.size.x, cursorPosition.x),
                    Mathf.InverseLerp(0, Terrain.terrainData.size.z, cursorPosition.z));
                ActualTrap.TrapPrefab.transform.rotation = Quaternion.LookRotation(Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y), Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
                ActualTrap.TrapPrefab.transform.position = cursorPosition;

                if (Input.GetMouseButtonDown(0) && GameManager.instance.SpendGold(ActualTrap.UpgradeCosts[0]))
                    CreateTrap();
                if (Input.GetMouseButtonDown(1))
                {
                    IsInTrapCreationMode = false;
                    UpdateUi(TrapTypes.None);
                    ActualSelectedTrapTypes = TrapTypes.None;
                    Destroy(ActualTrap.TrapPrefab);
                }
            }
            else if (IsTargetable() && _gameManager.IsTheSunAwakeAndTheBirdAreSinging)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    TargetedTrap.LevelUp();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    var cl = TargetedTrap;
                    TargetedTrap = null;
                    cl.Destroy();
                }
            }
        }
        void OnDisable()
        {
            IsInTrapCreationMode = false;
            ActualSelectedTrapTypes = TrapTypes.None;
            TargetedTrap = null;
            if (ActualTrap != null)
                Destroy(ActualTrap.TrapPrefab);
        }
        public void CreateTrapPrevu()
        {
            Trap trap = Traps[(int)SelectedTrapType];
            trap.IsInPreviewMode = true;
            GameObject trapGameObject = trap.TrapPrefab;
            trapGameObject.transform.position = GetCursorPosition();
            trapGameObject = Instantiate(trapGameObject);
            ActualTrap = (Trap)trapGameObject.GetComponentInChildren(trap.GetType());
        }
        public void CreateTrap()
        {
            if (Math.Abs(ActualTrap.TrapPrefab.GetComponentInChildren<Renderer>().material.color.r - 205) > 0.1)
            {
                Trap t = Traps[(int)SelectedTrapType];
                GameObject trap = Instantiate(t.TrapPrefab);
                ((Trap)trap.GetComponentInChildren(typeof(Trap))).IsInPreviewMode = false;
                foreach (var rend in trap.GetComponentsInChildren<Renderer>())
                {
                    var newMaterial = new Material(rend.material);
                    newMaterial.color = Color.grey;
                    rend.material = newMaterial;
                }
                Vector3 mousePosition = GetCursorPosition();
                var normalizedPos = new Vector2(Mathf.InverseLerp(0f, Terrain.terrainData.size.x, mousePosition.x),
                    Mathf.InverseLerp(0, Terrain.terrainData.size.z, mousePosition.z));
                trap.transform.rotation =
                    Quaternion.LookRotation(Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y),
                        Terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y));
                trap.transform.position = GetCursorPosition();
                GameOverManager.instance.PlacedTraps.Add(1);
                GameOverManager.instance.goldPerTrap[(int) SelectedTrapType] += ActualTrap.UpgradeCosts[0];
            }
        }

        public static Boolean IsTargetable()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f));
            Physics.Raycast(ray, out hitInfo, 100);
            float distance = Vector3.Distance(hitInfo.point, PlayerGameObject.transform.position);
            if (distance <= ActionRange && (hitInfo.collider.tag == "Trap" || hitInfo.collider.tag == "Leurre") && GameManager.instance.IsTheSunAwakeAndTheBirdAreSinging)
            {
                TargetedTrap = hitInfo.collider.gameObject.GetComponentInChildren<Trap>();
                TargetedTrap.Select();
                return true;
            }
            TargetedTrap = null;
            return false;
        }
        public static Vector3 GetCursorPosition()
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f));
            Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
            float distance = Vector3.Distance(hitInfo.point, PlayerGameObject.transform.position);

            if (distance <= ActionRange && hitInfo.point != Vector3.zero && hitInfo.collider.gameObject.tag != "Enclos")
            {
                return hitInfo.point;
            }
            if (hitInfo.point != Vector3.zero)
            {
                Vector3 positionAtRange = ray.direction * ActionRange + PlayerGameObject.transform.position;
                return new Vector3(positionAtRange.x, Terrain.SampleHeight(positionAtRange), positionAtRange.z);
            }
            else
            {
                Vector3 positionAtRange = ray.direction * ActionRange + PlayerGameObject.transform.position;
                return new Vector3(positionAtRange.x, Terrain.SampleHeight(positionAtRange), positionAtRange.z);
            }
        }

        public void UpdateUi(TrapTypes trapTypes)
        {
            if (SelectedTrapType != TrapTypes.None)
               transform.GetChild((int)SelectedTrapType).GetComponent<SelectionElement>().Unselect();
            if (trapTypes != TrapTypes.None)
                transform.GetChild((int)trapTypes).GetComponent<SelectionElement>().Select();
            SelectedTrapType = trapTypes;
        }
    }
}
