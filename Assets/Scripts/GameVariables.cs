﻿public static class GameVariables
{
    public static class Wolf
    {
        public static class Forest
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 1;
            public static readonly int farmerDamage = 1;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Water
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 1;
            public static readonly int farmerDamage = 1;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Mountain
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 1;
            public static readonly int farmerDamage = 1;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }

        public static class Boss
        {
            public static readonly int life = 100;
            public static readonly int enclosureDamage = 1;
            public static readonly int farmerDamage = 1;
            public static readonly int gold = 30;
            public static readonly float range = 1.0f;
        }
    }

    public static class Trap
    {
        public static class NeedleTrap
        {
            public static class Lv1
            {
                public static readonly int durability = 100;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv2
            {
                public static readonly int durability = 100;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv3
            {
                public static readonly int durability = 100;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }
        }

        public static class LandMine
        {
            public static class Lv1
            {
                public static readonly float radius = 1.0f;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv2
            {
                public static readonly float radius = 1.0f;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv3
            {
                public static readonly float radius = 1.0f;
                public static readonly int wolfDamage = 1;
                public static readonly int playerDamage = 1;
                public static readonly int upgradePrice = 100;
            }
        }

        public static class Decoy
        {
            public static class Lv1
            {
                public static readonly int life = 100;
                public static readonly float speed = 1.0f;
                public static readonly float radius = 1.0f;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv2
            {
                public static readonly int life = 100;
                public static readonly float speed = 1.0f;
                public static readonly float radius = 1.0f;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv3
            {
                public static readonly int life = 100;
                public static readonly float speed = 1.0f;
                public static readonly float radius = 1.0f;
                public static readonly int upgradePrice = 100;
            }
        }

        public static class Mud
        {
            public static class Lv1
            {
                public static readonly int durability = 100;
                public static readonly int wolfSlow = 10;
                public static readonly int playerSlow = 10;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv2
            {
                public static readonly int durability = 100;
                public static readonly int wolfSlow = 10;
                public static readonly int playerSlow = 10;
                public static readonly int upgradePrice = 100;
            }

            public static class Lv3
            {
                public static readonly int durability = 100;
                public static readonly int wolfSlow = 10;
                public static readonly int playerSlow = 10;
                public static readonly int upgradePrice = 100;
            }
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

    //Player multiplier
    public static class AchievementBonus
    {

    }

    public static class Round
    {

    }
    public static class Sheep
    {
        //public static readonly float flyDuration = 20f;
        public static readonly float flySpeed = 1f;
        public static readonly float walkSpeed = 2f;
        public static readonly float distMaxSound = 15f;
        public static readonly float distMinSound = 5f;
        public static readonly float volumeSound = .05f;
        public static readonly float volumeSoundDeath = .15f;
        public static readonly float volumeMusicSky = 1f;
        public static readonly string stringSheepSound = "sheepSound";
        public static readonly string stringSheepSoundDeath = "SheepDeath"; 
    }
    public static class Cycle
    {
        public static readonly float dayDuration = 300f;
        public static readonly float nightDuration = 300f;
        public static readonly float volumeThemes = .2f;
        public static readonly float volumeEffects = .1f;
        public static readonly float volumeVoice = .8f;
        public static readonly float passedCycleSpeed = 50f;
    }
}
