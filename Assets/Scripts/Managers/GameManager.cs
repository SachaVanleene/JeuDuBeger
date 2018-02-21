using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Script.Managers
{
    public class GameManager : MonoBehaviour, INewCycleListner
    {

        public static GameManager instance = null;
        public Text MessageText;
        public int TotalSheeps { get; set; }
        public List<EnclosManager> Paddocks;
        public GameObject Player;
        public GameObject CycleManagerObject;


        private SoundManager soundManager;
        private CycleManager cycleManager;
        private int gold;
        private int _roundNumber = 0;    

        private void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene -> only used if we want to keep state between two scenes
            //DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            soundManager = gameObject.GetComponent<SoundManager>();
            cycleManager = CycleManagerObject.GetComponent<CycleManager>();
            cycleManager.SubscribCycle(this);
            cycleManager.GoToAngle(1, 30);
            TotalSheeps = 10;
            DayStart();
        }

        void Update()
        {           
        }
                   
        private void placeSheeps() // place remaining sheeps in paddocks automatically
        {
            foreach(var p in Paddocks)
            {
                int i = 0;
                while(TotalSheeps > 0)
                {
                    if (i++ >= 10) // can't place more than 10 sheeps
                        break;
                    p.AddSheep();
                    TotalSheeps--;
                }                
            }
        }
        private void removeAllSheeps()
        {
            foreach (var p in Paddocks)
            {
                while (p.NbSheep > 0)
                {
                    p.RemoveSheep();
                    TotalSheeps++;
                }
            }
        }
        private void getGolds()
        {
            foreach(var p in Paddocks)
            {
                int toBeAdded = Mathf.RoundToInt(p.NbSheep - (2 * Mathf.Log(p.NbSheep)) * p.RewardGold);
                if (toBeAdded != (int)(p.NbSheep - (2 * Mathf.Log(p.NbSheep)) * p.RewardGold)) //  round up
                    toBeAdded++;
                this.gold += toBeAdded;
            }
            removeAllSheeps();
        }

        public void DayStart()
        {
            soundManager.PlayAmbuanceMusic("day_theme", 0.2f);
            soundManager.PlaySound("safe_place_to_rest", 1.5f);
            soundManager.PlaySound("bird", 0.2f);

            _roundNumber++;
            //TODO: Enable traps placement, sheeps interactions, player control. Start day light.
            MessageText.text = "ROUND " + _roundNumber;

            getGolds();
            cycleManager.GoToAngle(10, 181);
            //cycleManager.GoToAngle(175 / 600, 175); //  takes aprox 5min to end the day
        }

        public void NightStart()
        {
            soundManager.PlayAmbuanceMusic("night_theme", .2f);
            soundManager.PlaySound("wolf", 0.2f);
            soundManager.PlaySound("dont_fuck_with_me", 1.5f);
            //TODO: Disable traps placement, sheeps interactions. Start night light.

            cycleManager.GoToAngle(10, 1);

            //cycleManager.GoToAngle(175 / 600, 355); //  takes aprox 5min to end the night
        }

        public void WaitingAt(int goal, int angle)
        {
            MessageText.text = "waiting at " + angle + ", while aiming " + goal;
            // can do some verification, start a new wave, etc.
        }
        public bool SpendGold(int value)
        { //  allow the player to purchase
            if (value > gold)
                return false;
            gold -= value;
            // TODO : call achievement gold spent
            return true;
        }
    }
}