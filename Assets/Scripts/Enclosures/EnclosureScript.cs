using System;
using System.Collections.Generic;
using Assets.Script;
using Assets.Script.Managers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enclosures
{
    public class EnclosureScript : MonoBehaviour
    {
        public List<AudioClip> Clips;
        public GameObject PinkSuperSheepPrefab;
        public GameObject SheepPrefab;
        public Material PinkFence;
        public Material DefaultFence;
        public int GoldReward = GameVariables.EnclosureGold.close;
        public float Distance;
        public delegate void OnDead();
        public OnDead OnTriggerDead;
        public int Order;

        private AudioPlayerEnclosure audioPlayer;
        private bool _isDisplayingPanel = false;
        private GameManager _gameManager;
        private List<GameObject> _sheeps = new List<GameObject>();
        private List<GameObject> _superSheeps = new List<GameObject>();
        // for the flying sheeps
        public List<GameObject> FlyingSheeps = null;

        private GameObject _player;

        public int getNbSheepFlying()
        {   // used to know how many sheeps are to be sold
            return (FlyingSheeps != null)? FlyingSheeps.Count : 0;
        }

        public int SheepNumber
        {
            get { return _sheeps.Count; }
            set
            {
                if (value < 0)
                    return;
                EnclosureManager.SheepNumberInTheWorld = EnclosureManager.SheepNumberInTheWorld - _sheeps.Count + value;
                for (int i = 0; i < value - _sheeps.Count; i++)
                {
                    AddSheep();
                }
                for (int i = 0; i > value - _sheeps.Count; i--)
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
                    int sheepToRemove = SheepNumber - (int)Math.Truncate((double)value / 10);
                    for (int i = 0; i < sheepToRemove; i++)
                        KillSheep();
                }
                if (_health < value && (_health % 10) + (value - _health) >= 10 && value != 0)
                {
                    int sheepToAdd = (int)Math.Truncate((double)value / 10) - SheepNumber;
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
            _player = GameObject.FindGameObjectWithTag("Player");
            audioPlayer = gameObject.AddComponent(typeof(AudioPlayerEnclosure)) as AudioPlayerEnclosure;
            audioPlayer.ownerObject = this.gameObject;
            audioPlayer.Clips = this.Clips;
        }


        void Update()
        {
            ShowPanel();
        }

        private void ShowPanel()
        {
            Vector3 distPlayertoEnclos = _player.transform.position - transform.position;

            if (distPlayertoEnclos.magnitude < 25 && _gameManager.IsTheSunAwakeAndTheBirdAreSinging && !_gameManager.IsPaused)
            {
                if (!EnclosureManager.EnclosurePannel.activeSelf)
                {
                    EnclosureManager.EnclosurePannel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
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
                EnclosureManager.EnclosurePannel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                    SheepNumber.ToString();
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                if (Health != 0) _gameManager.TakeSheep();
                Health -= 10;
                EnclosureManager.EnclosurePannel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
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
                Health = 0; //santé min
                OnTriggerDead.Invoke();
                OnTriggerDead = null; //On reset le delegate
                if (_superSheeps.Count < 1 && _gameManager.TotalSuperSheeps >= 1)
                {
                    AddPinkSuperSheep();
                }
            }
        }
        public void AddSheep()
        {
            if (_gameManager.TotalSheeps < 0)
                return;
            if (SheepNumber < 10)
            {
                var sheep = Instantiate(SheepPrefab, this.transform); //crée un clone mouton
                sheep.transform.position = this.transform.position; //le place dans l'enclos
                sheep.transform.Rotate(0, Random.Range(0, 360), 0); //l'oriente d'une façon aléatoire
                _sheeps.Add(sheep);
                EnclosureManager.MiniMap.UpdateEnclosure(Order);
            }
            _gameManager.PlaceSheep();
        }
        public void RemovePinkSuperSheep()
        {
            var SuperSheep = _superSheeps[_superSheeps.Count - 1];
            SuperSheep.GetComponent<SheepBehaviour>().Kill();
            _superSheeps.Remove(SuperSheep);

            foreach (Transform child in transform)
            {
                if (child.tag == "Fences")
                {
                    Renderer r = child.GetComponent<Renderer>();
                    r.material = DefaultFence;
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
                    r.material = PinkFence;
                }
            }
        }

        public void KillSheep()
        {
            if (SheepNumber > 0)
            {
                var sheepClone = _sheeps[_sheeps.Count - 1]; //transform.GetChild(transform.childCount - SheepNumber).gameObject;
                sheepClone.GetComponent<SheepBehaviour>().Kill();
                /*GameObject boom = CFX_SpawnSystem.GetNextObject(SmokeEffect);
                boom.transform.position = sheepClone.transform.position;
                Destroy(sheepClone);*/
                _sheeps.Remove(sheepClone);
                EnclosureManager.MiniMap.UpdateEnclosure(Order);
                // only call it if the sheep has been killed by a wolf, not removed from panel
                if (!_gameManager.IsTheSunAwakeAndTheBirdAreSinging)
                    _gameManager.KillSheep();
            }
        }
        public void RemoveAllSheeps()
        {
           if(FlyingSheeps != null)
            {
                audioPlayer.Stop();
                foreach (var sheep in FlyingSheeps)
                {
                    sheep.GetComponent<SheepBehaviour>().DestroyMe();
                }
                FlyingSheeps = null;
            }
        }
        public void MakeSheepsFly()
        {
            _gameManager.TotalSheeps += _sheeps.Count;
            if (_superSheeps.Count > 0)
                RemovePinkSuperSheep();
            foreach(var sheep in _sheeps)
            {
                sheep.GetComponent<SheepBehaviour>().SpiritInTheSky();
            }
            FlyingSheeps = _sheeps;
            if(FlyingSheeps.Count > 0)
                audioPlayer.PlaySound("spirit", GameVariables.Sheep.volumeMusicSky);
            _sheeps = new List<GameObject>();
            _health = 0;
            EnclosureManager.MiniMap.UpdateEnclosure(Order);

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

