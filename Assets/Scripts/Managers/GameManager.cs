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
        public Text TextRounds;
        public Text TextGolds;
        public Text TextSheeps;
        public GameObject TextInfo;
        public GameObject CycleManagerObject;
        public GameObject Spawns;
        public GameObject PanelBackToMenu;
        public GameObject GameOverChart;
                
        public int TotalSheeps { get; set; }    // player inventory relativ
        public int TotalSuperSheeps { get; set; }
        public bool IsTheSunAwakeAndTheBirdAreSinging { get; set; }
        public bool IsPaused { get; set; }

        private EnclosureManager _enclosureManager;
        private SoundManager soundManager;
        private CycleManager cycleManager;
        private int _roundNumber = 0;
        private bool cheatsActivated = false;
        
        public bool gameOver = false;
        private int gold = 0;   // player inventory relativ


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            IsPaused = false;
            IsTheSunAwakeAndTheBirdAreSinging = true;
        }

        private void Start()
        {
            soundManager = gameObject.GetComponent<SoundManager>();
            _enclosureManager = EnclosureManager.Instance;
            cycleManager = CycleManagerObject.GetComponent<CycleManager>();
            cycleManager.SubscribCycle(this);
            cycleManager.GoToAngle(1, 30);
            TotalSheeps = 15;
            
            if(SProfilePlayer.getInstance().AchievementsManager.GetAchievementByName("Player").IsComplete())
                TotalSuperSheeps = 1;
            else
                TotalSuperSheeps = 0;
            DayStart();
            Cursor.visible = false;
        }

        private void Update()
        {
            if (gameOver)   // can't unpaused when game is over
                return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                if (IsPaused)
                    UnPauseGame();
                else
                    PauseGame();
                return;
            }
            if (IsPaused)   // can only unpaused from pause
                return;
            if (Input.GetKeyUp("n") && IsTheSunAwakeAndTheBirdAreSinging)
            {
                cycleManager.NextCycle(50f);
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                displayInfo("cheats activés", 1);
            }
            else
                return;

            // cheats here
            if (Input.GetKeyUp("n") && !IsTheSunAwakeAndTheBirdAreSinging)
            {
                callAchievement(AchievementEvent.cheat);
                while (Spawns.GetComponent<Spawn_wolf>().hasWolfAlive())
                    this.DeathWolf();
            }
            if (Input.GetKeyUp("e"))
            {
                callAchievement(AchievementEvent.cheat);
                earnGold(150);
            }
            if (Input.GetKeyUp("l"))
            {
                callAchievement(AchievementEvent.cheat);
                SpendGold(150);
            }
            if (Input.GetKeyUp("p"))
            {
                callAchievement(AchievementEvent.cheat);
                TextInfo.GetComponent<InfoTextScript>().Clear();
                GameOver();
            }
            if (Input.GetKeyUp("m"))
            {
                callAchievement(AchievementEvent.cheat);
                TotalSuperSheeps++;
                printNbSHeeps();
            }
        }

        public void UnPauseGame()
        {
            if (gameOver)
                return;
            Cursor.visible = false;
            IsPaused = false;
            Time.timeScale = 1;
            PanelBackToMenu.SetActive(false);
        }
        public void PauseGame()
        {
            Cursor.visible = true;
            IsPaused = true;
            
            Time.timeScale = 0;
            PanelBackToMenu.SetActive(true);
            PanelBackToMenu.GetComponent<ScriptBackToMenuPanel>().Pause();
        }
        public void GameOver()
        {
            callAchievement(AchievementEvent.lose);
            IsPaused = true;
            gameOver = true;
            Cursor.visible = true;
            Time.timeScale = 0;
            PanelBackToMenu.SetActive(true);
            GameOverChart.SetActive(true);
            PanelBackToMenu.GetComponent<ScriptBackToMenuPanel>().Pause(gameOver : true);
        }
        public void BackToMenu()
        {
            callAchievement(AchievementEvent.quit);
            SceneManager.LoadScene("MainMenu");
        }
        public void KillSheep()
        {
            callAchievement(AchievementEvent.sheepDeath);
            displayInfo("A sheep has been eaten", 2);
            if (EnclosureManager.NbSheeps() <= 0)
                GameOver();
        }
        public void TakeSheep()
        {
            TotalSheeps++;
            printNbSHeeps();
        }
        public void PlaceSheep()
        {
            TotalSheeps--;
            printNbSHeeps();
        }
        public void PlaceSuperSheep()
        {
            TotalSuperSheeps--;
            printNbSHeeps();
        }
        private void printNbSHeeps()
        {
            if (TotalSuperSheeps > 0)
                TextSheeps.text = TotalSheeps + " Sheeps in Inventory and " + TotalSuperSheeps + " Super.";
            else
                TextSheeps.text = TotalSheeps + " Sheeps in Inventory.";
        }
        private void getGoldsRound()
        {
            foreach (var p in _enclosureManager.EnclosPrefabList)
            {
                if (p.SheepNumber <= 0)
                    continue;
                int toBeAdded = Mathf.RoundToInt(p.SheepNumber - (2 * Mathf.Log(p.SheepNumber)) * p.SheepNumber);
                if (toBeAdded != (int)(p.SheepNumber - (2 * Mathf.Log(p.SheepNumber)) * p.GoldReward)) //  round up
                    toBeAdded++;
                earnGold(toBeAdded);
            }
            TextGolds.text = gold + " gold";
        }
        private void displayInfo(string msg, int duration)
        {
            TextInfo.GetComponent<InfoTextScript>().DisplayInfo(msg, duration);
        }
        private void newRound()
        {
            Spawns.GetComponent<Spawn_wolf>().Cycle = ++_roundNumber;
            TextRounds.text = "ROUND " + _roundNumber;
            displayInfo("Round " + _roundNumber + " begin \n Press n to pass directly to the night", 5);
            callAchievement(AchievementEvent.cycleEnd);
        }
        public void DayStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = true;
           if(_roundNumber != 0) _enclosureManager.TakeOffAllSheeps();
            printNbSHeeps();
            soundManager.PlayAmbuanceMusic("day_theme", 0.2f);
            soundManager.PlaySound("safe_place_to_rest", 1.5f);
            soundManager.PlaySound("bird", 0.2f);

            newRound();
            getGoldsRound();
            cycleManager.GoToAngle(180f/360f, 181); //  takes aprox 5min to end the day
        }
        public void NightStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = false;
            soundManager.PlayAmbuanceMusic("night_theme", .2f);
            soundManager.PlaySound("wolf", 0.2f);
            soundManager.PlaySound("dont_fuck_with_me", 1.5f);

            Spawns.GetComponent<Spawn_wolf>().Begin_Night();

            _enclosureManager.DefaultFilling();
            cycleManager.GoToAngle(180f / 600f, 355); //  takes aprox 5min to end the night
        }
        public void WaitingAt(int goal, int angle)
        {
            TextRounds.text = "waiting at " + angle + ", while aiming " + goal;
            // can do some verification, start a new wave, etc.
            if(goal == 355 && Spawns.GetComponent<Spawn_wolf>().hasWolfAlive())
                displayInfo("The Night will end only when the wolfs are dead", 4);
        }
        private void earnGold(int value)
        {
            gold += value;
            TextGolds.text = gold + " gold";
            callAchievement(AchievementEvent.goldEarn, value);
        }
        public bool SpendGold(int value)
        { //  allow the player to purchase
            if (value > gold)
                return false;
            gold -= value;
            TextGolds.text = gold + " gold";

            callAchievement(AchievementEvent.goldSpent, value);
            return true;
        }
        public void DeathPersonnage()
        {
            callAchievement(AchievementEvent.playerDeath);
        }
        public void DeathWolf(GameObject wolf = null)
        {
            callAchievement(AchievementEvent.wolfDeath);
            Spawns.GetComponent<Spawn_wolf>().WolfDeath(wolf);
            if (!Spawns.GetComponent<Spawn_wolf>().hasWolfAlive())
                cycleManager.NextCycle(25f);
        }
        private void callAchievement(AchievementEvent achEvent, int step = 1)
        {
            SProfilePlayer.getInstance().AchievementsManager.AddStepAchievement(achEvent, step);
        }
    }
}