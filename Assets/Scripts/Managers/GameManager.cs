using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
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
        public GameObject Canvas;
        public GameObject TextInfo;
        public GameObject CycleManagerObject;
        public GameObject Spawns;
        public GameObject PanelBackToMenu;
        public GameObject GameOverChart;
        public GameObject TrapsCreationPannel;
        public GameObject AchievementPopUp;
        public GameObject PanelWolvesAliveInRound;
        public GameObject SheepsInInventory;
        public List<GameObject> VisualPanels; // panels to be hidden when in pause

        public SO.GameEvent goldChange;
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
        private int gold = 30;   // player inventory relativ
        private Dictionary<GameObject, bool> previousState = new Dictionary<GameObject, bool>();

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
            TotalSheeps = GameVariables.Initialisation.numberSheeps;
            Time.timeScale = 1;
            GetComponent<DifficultyManager>().SetDiffilculty();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (SProfilePlayer.getInstance().AchievementsManager.GetAchievementByName("Player").IsComplete())
                TotalSuperSheeps = 1;
            else
                TotalSuperSheeps = 0;
            DayStart();
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
                cycleManager.NextCycle(GameVariables.Cycle.passedCycleSpeed);
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                displayInfo(Strings.IngameInterface["ActivatedCheats"], 1);
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
                EarnGold(150, false);
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

        private void hideAllPanels()
        {
            foreach(var panel in VisualPanels)
            {
                previousState[panel] = panel.activeSelf;
                panel.SetActive(false);
            }
        }
        private void reEnablePanels()
        {
            foreach (var panel in VisualPanels)
            {
                panel.SetActive(previousState[panel]);
            }
        }

        public void UnPauseGame()
        {
            if (gameOver)
                return;
            reEnablePanels();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsPaused = false;
            Time.timeScale = 1;
            PanelBackToMenu.SetActive(false);
        }
        public void PauseGame()
        {
            hideAllPanels();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsPaused = true;

            Time.timeScale = 0;
            PanelBackToMenu.SetActive(true);
            PanelBackToMenu.GetComponent<ScriptBackToMenuPanel>().Pause();
        }
        public void GameOver()
        {
            hideAllPanels();
            Canvas.GetComponent<PauseTuto>().GameOver();
            callAchievement(AchievementEvent.lose);
            IsPaused = true;
            gameOver = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            PanelBackToMenu.SetActive(true);

            GameOverManager.instance.SetFavoriteEnclosure();
            GameOverManager.instance.SetFavoriteTrap();

            GameOverChart.SetActive(true);

            PanelBackToMenu.GetComponent<ScriptBackToMenuPanel>().Pause(gameOver : true);
        }
        public void BackToMenu()
        {
            if(!gameOver)
                callAchievement(AchievementEvent.quit);
            SceneManager.LoadScene("MainMenu");
        }
        public void KillSheep()
        {
            callAchievement(AchievementEvent.sheepDeath);
            displayInfo(Strings.IngameInterface["SheepEaten"], 2);
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
                TextSheeps.text = TotalSheeps + Strings.IngameInterface["SheepsInInventoryAnd"] + TotalSuperSheeps + Strings.IngameInterface["SuperSheeps"];
            else
                TextSheeps.text = TotalSheeps + Strings.IngameInterface["SheepsInInventory"];
        }
        private void getGoldsRound()
        {
            foreach (var p in EnclosureManager.EnclosureList)
            {
                int nb = p.getNbSheepFlying();
                if (nb <=  0)
                    continue;

                int toBeAdded = Mathf.RoundToInt((nb - (2 * Mathf.Log(nb))) * p.GoldReward);
                if (toBeAdded != (int)((nb - (2 * Mathf.Log(nb))) * p.GoldReward)) //  round up
                    toBeAdded++;
                EarnGold(toBeAdded);

                if (p.GoldReward == GameVariables.EnclosureGold.close)
                    GameOverManager.instance.goldPerEnclosure[0] += toBeAdded;
                else if (p.GoldReward == GameVariables.EnclosureGold.medium)
                    GameOverManager.instance.goldPerEnclosure[1] += toBeAdded;
                else
                    GameOverManager.instance.goldPerEnclosure[2] += toBeAdded;
            }
            TextGolds.text = gold + Strings.IngameInterface["Gold"];
        }
        private void displayInfo(string msg, int duration)
        {
            TextInfo.GetComponent<InfoTextScript>().DisplayInfo(msg, duration);
        }
        private void newRound()
        {
            if (_roundNumber != 0)
                callAchievement(AchievementEvent.cycleEnd);
            Spawns.GetComponent<Spawn_wolf>().Cycle = ++_roundNumber;
            if (_roundNumber % GameVariables.Round.periodicityEarnSheep == 0)
                TotalSheeps += GameVariables.Round.quantityEarnSheepPeriodically;
            TextRounds.text = Strings.IngameInterface["Round"] + _roundNumber;
            Debug.LogError(Strings.IngameInterface["Round"] + _roundNumber + Strings.IngameInterface["PassDay"]);
            displayInfo(Strings.IngameInterface["Round"] + _roundNumber + Strings.IngameInterface["PassDay"], 5);

        }
        public void DayStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = true;
            newRound();
            soundManager.PlayAmbuanceMusic("day_theme", GameVariables.Cycle.volumeThemes);
            soundManager.PlaySound("safe_place_to_rest", GameVariables.Cycle.volumeVoice);
            soundManager.PlaySound("bird", GameVariables.Cycle.volumeEffects);
            printNbSHeeps();
            getGoldsRound();
            cycleManager.GoToAngle((GameVariables.Cycle.duskAngle - cycleManager.GetCurentCycle()) / GameVariables.Cycle.dayDuration,
                (int)GameVariables.Cycle.duskAngle);
            TrapsCreationPannel.SetActive(true);
            PanelWolvesAliveInRound.SetActive(false);
            SheepsInInventory.SetActive(true);
            if (_roundNumber == 1)
            {
                TutorialOpen.instance.OpenTutorial(GameVariables.Tutorials.howToPlay);
            }
            if(_roundNumber == 2)
            {
                TutorialOpen.instance.OpenTutorial(GameVariables.Tutorials.golds);
            }
        }
        public void NightStart()
        {
            IsTheSunAwakeAndTheBirdAreSinging = false;
            // destroy flying sheeps
            _enclosureManager.TakeOffAllSheeps();

            soundManager.PlayAmbuanceMusic("night_theme", GameVariables.Cycle.volumeThemes);
            soundManager.PlaySound("dont_fuck_with_me", GameVariables.Cycle.volumeVoice);
            soundManager.PlaySound("wolf", GameVariables.Cycle.volumeEffects);
            Spawns.GetComponent<Spawn_wolf>().Begin_Night();
            _enclosureManager.DefaultFilling();
            cycleManager.GoToAngle((GameVariables.Cycle.dawnAngle - cycleManager.GetCurentCycle()) / GameVariables.Cycle.nightDuration,
                (int)GameVariables.Cycle.dawnAngle);
            TrapsCreationPannel.SetActive(false);
            PanelWolvesAliveInRound.SetActive(true);
            SheepsInInventory.SetActive(false);
            TutorialOpen.instance.OpenTutorial(GameVariables.Tutorials.wolfs);
        }
        public void WaitingAt(int goal, int angle)
        {
            if(goal != angle)
                Debug.LogError("cycle waiting at " + angle + ", while aiming " + goal);
            // can do some verification, start a new wave, etc.
            if(goal == 355 && Spawns.GetComponent<Spawn_wolf>().hasWolfAlive())
                displayInfo(Strings.IngameInterface["NightEndCondition"], 4);
        }
        public void EarnGold(int value, bool enclosureGold = true, bool wolfGold = false, bool callAch = true)
        {
            gold += value;
            TextGolds.text = gold + Strings.IngameInterface["Gold"];
            if(callAch)
                callAchievement(AchievementEvent.goldEarn, value);

            GameOverManager.instance.TotalGoldEarned.Add(value);
            GameOverManager.instance.GoldChange.Set("+" + value);
            goldChange.Raise();

            if (enclosureGold)
                GameOverManager.instance.EnclosureGold.Add(value);
            if(wolfGold)
                GameOverManager.instance.WolvesGold.Add(value);
        }
        public bool SpendGold(int value)
        { //  allow the player to purchase
            if (value > gold)
            {
                displayInfo(Strings.IngameInterface["NotEnoughGold"], 1);
                return false;
            }
            gold -= value;
            TextGolds.text = gold + Strings.IngameInterface["Gold"];

            GameOverManager.instance.GoldChange.Set("-" + value);
            goldChange.Raise();
            GameOverManager.instance.GoldSpent.value -= value;

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
            {
                _enclosureManager.SheepsToTheSky();
                cycleManager.NextCycle(GameVariables.Cycle.passedCycleSpeed);
            }
        }
        private void callAchievement(AchievementEvent achEvent, int step = 1)
        {
            List<AchievementInfo> endedAchievements = SProfilePlayer.getInstance().AchievementsManager.AddStepAchievement(achEvent, step);
            // hack fix to prevent bug when quitting the game
            if (achEvent == AchievementEvent.quit)
                return;
            if(endedAchievements != null)
            {
                foreach(var achInfo in endedAchievements)
                {
                    AchievementPopUp.GetComponent<AchievementPopUpScript>().AddAchievement(achInfo);
                }
            }
        }

        public int GetCycle()
        {
            return _roundNumber;
        }
    }
}