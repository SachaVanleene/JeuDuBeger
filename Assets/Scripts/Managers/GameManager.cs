using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Managers
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;


        private int _roundNumber;                 // Which round the game is currently on.
        private WaitForSeconds _startWait;         // Used to have a delay whilst the round starts.
        private WaitForSeconds _endWait;           // Used to have a delay whilst the round or game ends.
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the first level 
            //InitGame();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}