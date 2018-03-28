using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class GameVariables
    {
        public static class Wolf
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 10;
            public static readonly int farmerDamage = 10;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;


            public static readonly float distMaxSound = 30f;
            public static readonly float volumeSoundDeath = .1f; 
            public static readonly float volumeSoundAttack = .5f;
            public static readonly string stringSoundHit = "hit";
            public static readonly int NbDifferentSoundAttack = 3; 
            public static readonly string stringSoundAttack = "attack"; 
            public static readonly string stringSoundBlizzard = "blizzard";
        }

        public static class Water
        {
            public static readonly int life = 100;
            public static readonly float enclosureDamage = 0.1f;
            public static readonly float farmerDamage = 0.1f;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Mountain
        {
            public static readonly int life = 100;
            public static readonly float enclosureDamage = 0.1f;
            public static readonly float farmerDamage = 0.1f;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Boss
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 10;
            public static readonly int farmerDamage = 10;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Trap
        {
            public static class NeedleTrap
            {
                public static readonly int durability = 100;
                public static readonly int wolfDamage = 1;
                public static readonly List<int> playerDamage = new List<int>() {5, 10, 20};
                public static readonly List<int> upgradePrice = new List<int>() {20, 50, 100};
            }

            public static class LandMine
            {
                public static readonly List<float> radius = new List<float>() {10, 15, 20};
                public static readonly int wolfDamage = 1;
                public static readonly List<int> playerDamage = new List<int>() {50, 75, 100};
                public static readonly List<int> upgradePrice = new List<int>() {5, 10, 20};
            }

            public static class Decoy
            {
                public static readonly int life = 100;
                public static readonly List<float> speed = new List<float>() {0.8f, 0.6f, 0.4f};
                public static readonly float radius = 1.0f;
                public static readonly List<int> upgradePrice = new List<int>() {20, 50, 100};
            }

            public static class Mud
            {
                public static readonly int durability = 100;
                public static readonly List<int> wolfSlow = new List<int>() {10, 20, 40};
                public static readonly int playerSlow = 10;
                public static readonly List<int> upgradePrice = new List<int>() {20, 50, 100};
            }
        }

        public static class Gun
        {
            public static class Lv1
            {
                public static readonly int wolfDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv2
            {
                public static readonly int wolfDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv3
            {
                public static readonly int wolfDamage = 1;
                public static readonly int upgradePrice = 100;
            }
        }

        public static class EnclosureGold
        {
            public static readonly int close = 1;

            public static readonly int medium = 2;

            public static readonly int far = 3;
        }

        //Wolf multiplier 
        public static class Difficulty
        {
            public static class Easy
            {
                public static readonly float life = 0.5f;
                public static readonly float enclosureDamage = 0.5f;
                public static readonly float playerDamage = 0.5f;
                public static readonly float gold = 1.5f;
            }

            public static class Normal
            {
                public static readonly float life = 1f;
                public static readonly float enclosureDamage = 1f;
                public static readonly float playerDamage = 1f;
                public static readonly float gold = 1f;
            }

            public static class Hard
            {
                public static readonly float life = 0.5f;
                public static readonly float enclosureDamage = 0.5f;
                public static readonly float playerDamage = 0.5f;
                public static readonly float gold = 1.5f;
            }
        }

        public static class Achievements
        {
            public static class Bonus
            {
                //  players boosts 

            }

            public static class PopUp
            {
                public static readonly float speedCome = 45f;
                public static readonly float speedBack = 35f;
                public static readonly float timeStay = 2f;
            }
        }

        public static class Round
        {
            public static readonly int periodicityEarnSheep = 3;
            public static readonly int quantityEarnSheepPeriodically = 5;

        }
        public static class Initialisation
        {
            public static readonly int numberSheeps = 15;

        }
        public static class Enclosure
        {
            public static readonly float distMaxSound = 45f;
            public static readonly float distMinSound = 0f;
            public static readonly float volumeMusicSky = 1f;
            public static readonly string stringEnclosureMusicFly = "spirit";
        }
        public static class Sheep
        {
            public static readonly float flySpeed = 1f;
            public static readonly float walkSpeed = 2f;
            public static readonly float distMaxSound = 30f;
            public static readonly float volumeSound = 1f;
            public static readonly float volumeSoundDeath = .1f;
            public static readonly string stringSheepSound = "sheepSound";
            public static readonly string stringSheepSoundDeath = "SheepDeath";
        }

        public static class Player
        {
            public static readonly float distMaxSound = 30f;
            public static readonly string stringSoundFreezing = "freezing";
        }

        public static class Cycle
        {
            public static readonly float dayDuration = 300f;
            public static readonly float nightDuration = 300f;
            public static readonly float dawnAngle = 355f;
            public static readonly float duskAngle = 180f;
            public static readonly float volumeThemes = .1f;
            public static readonly float volumeEffects = .05f;
            public static readonly float volumeVoice = .6f;
            public static readonly float passedCycleSpeed = 50f;
        }
    }
}

