using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script;
using Assets.Script.Managers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enclosures
{


    public class EnclosureScript : MonoBehaviour
    {

        public GameObject SmokeEffect;
        public GameObject PinkSuperSheepPrefab;
        public GameObject SheepPrefab;
        public Material pinkFence;
        public Material defaultFence;
        public int GoldReward = 1; // depend of the distance of the enclos
        public float Distance;
        public delegate void OnDead();
        public OnDead OnTriggerDead;


        private bool _isDisplayingPanel = false;
        private GameManager _gameManager;
        private List<GameObject> _sheeps = new List<GameObject>();
        private List<GameObject> _superSheeps = new List<GameObject>();

        public int SheepNumber
        {
            get { return _sheeps.Count; }
            set
            {
                if (value < 0)
                    return;
                EnclosureManager.SheepNumberInTheWorld = EnclosureManager.SheepNumberInTheWorld - _sheeps.Count + value;
                for(int i = 0; i < value - _sheeps.Count; i++)
                {
                    AddSheep();
                }
                for(int i = 0; i > value - _sheeps.Count; i--)
                {
                    KillSheep();
                }
            }
        }

        private float _health;
        public float Health
        {
            get { return _health; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > 100)
                {
                    return;
                }
                if (_health > value && (_health % 10) - (_health - value) <= 0 && _health != 0)
                {
                    int sheepToRemove = SheepNumber - (int) Math.Truncate((double) value / 10);
                    for (int i = 0; i < sheepToRemove; i++)
                        KillSheep();
                }
                if (_health < value && (_health % 10) + (value - _health) >= 10 && value != 0)
                {
                    int sheepToAdd = (int) Math.Truncate((double) value / 10) - SheepNumber;
                    for (var i = 0; i < sheepToAdd; i++)
                    {
                        AddSheep();
                    }
                }
                _health = value;
            }
        }

        private void Start()
        {
            Distance = Vector3.Distance(EnclosureManager.HousePosition, this.transform.position);
            _gameManager = GameManager.instance;
        }


    void Update()
        {
            ShowPanel();
        }

        private void ShowPanel()
        {
            Vector3 distPlayertoEnclos = TerrainTest.PlayerGameObject.transform.position - transform.position;

            if (distPlayertoEnclos.magnitude < 25 && _gameManager.IsTheSunAwakeAndTheBirdAreSinging && !_gameManager.IsPaused)
            {
                if (!EnclosureManager.EnclosurePannel.activeSelf)
                {
                    EnclosureManager.EnclosurePannel.transform.GetChild(1).GetComponent<Text>().text =
                        SheepNumber.ToString();
                    EnclosureManager.EnclosurePannel.SetActive(true);
                    _isDisplayingPanel = true;
                }
                HandleInputs();
            }
            else
            {
                if (EnclosureManager.EnclosurePannel.activeSelf && _isDisplayingPanel)
                {
                    _isDisplayingPanel = false;
                    EnclosureManager.EnclosurePannel.SetActive(false);
                }
            }
        }
        private void HandleInputs()
        {

            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                Health += 10;
                EnclosureManager.EnclosurePannel.transform.GetChild(1).GetComponent<Text>().text =
                    SheepNumber.ToString();
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (Health != 0) _gameManager.TakeSheep();
                Health -= 10;
                EnclosureManager.EnclosurePannel.transform.GetChild(1).GetComponent<Text>().text =
                    SheepNumber.ToString();
            }
            if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                if (_superSheeps.Count < 1 && _gameManager.TotalSuperSheeps >= 1)
                {
                    AddPinkSuperSheep();
                }
            }

    }

    public void DamageEnclos(float degats)
        {
            Health -= degats;
            if (Health == 0)
            {
                if (_superSheeps.Count < 1 && _gameManager.TotalSuperSheeps >= 1)
                {
                    AddPinkSuperSheep();
                }
            }
        }


        public void AddSheep()
        {
            if (_gameManager.TotalSheeps <= 0)
                return;
            if (SheepNumber < 10)
            {
                var sheep = Instantiate(SheepPrefab, this.transform); //crée un clone mouton
                sheep.transform.position = this.transform.position; //le place dans l'enclos
                sheep.transform.Rotate(0, Random.Range(0, 360), 0); //l'oriente d'une façon aléatoire
                _sheeps.Add(sheep);
            }
            _gameManager.PlaceSheep();
        }

        public void RemovePinkSuperSheep()
        {
            var SuperSheep = _superSheeps[_superSheeps.Count - 1];
            GameObject boom = CFX_SpawnSystem.GetNextObject(SmokeEffect);
            boom.transform.position = SuperSheep.transform.position;
            Destroy(SuperSheep);
            _superSheeps.Remove(SuperSheep);

            foreach (Transform child in transform)
            {
                if (child.tag == "Fences")
                {
                    Renderer r = child.GetComponent<Renderer>();
                    r.material = defaultFence;
                }
            }
        }

        public void AddPinkSuperSheep()
        {

            var SuperSheep = Instantiate(PinkSuperSheepPrefab, this.transform); //crée un clone mouton
            SuperSheep.transform.position = this.transform.position; //le place dans l'enclos
            SuperSheep.transform.Rotate(0, Random.Range(0, 360), 0);

            _superSheeps.Add(SuperSheep);
            
            _gameManager.PlaceSuperSheep();

            foreach (Transform child in transform)
            {
                if (child.tag == "Fences")
                {
                    Renderer r = child.GetComponent<Renderer>();
                    r.material = pinkFence;
                }
            }
        }

    public void KillSheep()
        {
            if (SheepNumber > 0)
            {
                var sheepClone = _sheeps[_sheeps.Count - 1]; //transform.GetChild(transform.childCount - SheepNumber).gameObject;
                GameObject boom = CFX_SpawnSystem.GetNextObject(SmokeEffect);
                boom.transform.position = sheepClone.transform.position;
                _sheeps.Remove(sheepClone);
                Destroy(sheepClone);
            }
        }
        public void RemoveAllSheeps()
        {
            _gameManager.TotalSheeps += _sheeps.Count;
            Health = 0;
            if (_superSheeps.Count > 0)
                RemovePinkSuperSheep();
        }
        public void DamageEnclos(int degats)
        {
            // takes no damage if protected by a super sheep
            if (_superSheeps.Count > 0)
                return;
            Health -= degats;
            if (Health == 0)
            {
                Health = 0; //santé min
                Debug.Log("Enclos Mort");
                OnTriggerDead.Invoke();
                OnTriggerDead = null; //On reset le delegate
            }

        }

        public void AddSubscriber(OnDead function)
        {
            OnTriggerDead += function;
        }
        public void RemoveSubscriber(OnDead function)
        {
            if (function != null)
            {
                OnTriggerDead -= function;
            }
        }

    }
}

