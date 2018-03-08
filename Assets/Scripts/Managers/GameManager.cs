﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Script.Managers
{
    public class GameManager : MonoBehaviour, INewCycleListner
    {

        public static GameManager instance = null;
        public Text TextRounds;
        public Text TextGolds;
        public Text TextSheeps;
        public GameObject TextInfo;
        public GameObject Player;
        public GameObject CycleManagerObject;
        public GameObject Spawns;
        public bool IsTheSunAwakeAndTheBirdAreSinging;

        private int _roundNumber = 0;
        private EnclosureManager _enclosureManager;

        private SoundManager soundManager;
        private CycleManager cycleManager;
        // player's inventory relativ
        private int gold = 0;
        public int TotalSheeps { get; set; } 


        private void Awake()
        {
            if (instance == null)
                instance = this;

            else if (instance != this)
                Destroy(gameObject);
        }


        private void Start()
        {
            soundManager = gameObject.GetComponent<SoundManager>();
            _enclosureManager = EnclosureManager.Instance;
            cycleManager = CycleManagerObject.GetComponent<CycleManager>();
            cycleManager.SubscribCycle(this);
            cycleManager.GoToAngle(1, 30);
            TotalSheeps = 15;
            DayStart();
        }

        public void KillSheep()
        {
            // achievement
            displayInfo("A sheep has been eaten", 2);
        }
        public void TakeSheep()
        {
            TotalSheeps++;
            TextSheeps.text = TotalSheeps + " Sheeps in Inventory";
        }
        public void PlaceSheep()
        {
            TotalSheeps--;
            TextSheeps.text = TotalSheeps + " Sheeps in Inventory";
        }
       
        private void getGoldsRound()
        {
            /**foreach (var p in Paddocks)
            {
                if (p.NbSheep <= 0)
                    continue;
                int toBeAdded = Mathf.RoundToInt(p.NbSheep - (2 * Mathf.Log(p.NbSheep)) * p.RewardGold);
                if (toBeAdded != (int)(p.NbSheep - (2 * Mathf.Log(p.NbSheep)) * p.RewardGold)) //  round up
                    toBeAdded++;
                earnGold(toBeAdded);
            }
            TextGolds.text = gold + " gold";
            removeAllSheeps();**/
        }

        private void displayInfo(string msg, int duration)
        {
            TextInfo.GetComponent<InfoTextScript>().DisplayInfo(msg, duration);
        }
        private void newRound()
        {
            Spawns.GetComponent<Spawn_wolf>().Cycle = ++_roundNumber;
            TextRounds.text = "ROUND " + _roundNumber;
            displayInfo("Round " + _roundNumber + " begin", 2);
            // calls achievements nb rounds
        }

        public void DayStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = true;
           if(_roundNumber != 0) _enclosureManager.TakeOffAllSheeps();
            TextSheeps.text = TotalSheeps + " Sheeps in Inventory";
            soundManager.PlayAmbuanceMusic("day_theme", 0.2f);
            soundManager.PlaySound("safe_place_to_rest", 1.5f);
            soundManager.PlaySound("bird", 0.2f);

            //TODO: Enable traps placement, sheeps interactions, player control. Start day light.
            newRound();
            getGoldsRound();
            cycleManager.GoToAngle(0.3f, 175); //  takes aprox 5min to end the day
        }

        public void NightStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = false;
            soundManager.PlayAmbuanceMusic("night_theme", .2f);
            soundManager.PlaySound("wolf", 0.2f);
            soundManager.PlaySound("dont_fuck_with_me", 1.5f);
            //TODO: Disable traps placement, sheeps interactions. Start night light.


            Spawns.GetComponent<Spawn_wolf>().Begin_Night();

            cycleManager.GoToAngle(0.3f, 355); //  takes aprox 5min to end the night
            _enclosureManager.DefaultFilling();
            //cycleManager.GoToAngle(175 / 600, 355); //  takes aprox 5min to end the night
        }

        public void WaitingAt(int goal, int angle)
        {
            TextRounds.text = "waiting at " + angle + ", while aiming " + goal;
            // can do some verification, start a new wave, etc.
            if(goal == 355 && Spawns.GetComponent<Spawn_wolf>().hasWolfAlive())
                displayInfo("The Night will only end when all the wolfs will be dead", 4);
        }
        private void earnGold(int value)
        {
            gold += value;
            TextGolds.text = gold + " gold";
            // call achievement gold earn
        }
        public bool SpendGold(int value)
        { //  allow the player to purchase
            if (value > gold)
                return false;
            gold -= value;
            TextGolds.text = gold + " gold";

            // TODO : call achievement gold spent
            return true;
        }
    }
}