﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameOverManager : MonoBehaviour {

    public static GameOverManager instance = null;

    [HideInInspector]
    public List<int> goldPerEnclosure = new List<int>(3);
    [HideInInspector]
    public List<int> goldPerTrap = new List<int>(4);

    [Header("Variables references")]
    public SO.IntVariable DeathCount;
    public SO.IntVariable PlayerDamageDealt;
    public SO.IntVariable GoldEarned;
    public SO.IntVariable TotalSheeps;

    public SO.IntVariable PlacedTraps;
    public SO.IntVariable TrapsDamageDealt;
    public SO.IntVariable GoldSpent;
    public SO.StringVariable FavoriteTrap;

    public SO.IntVariable Wolves;
    public SO.IntVariable Werewolves;
    public SO.IntVariable WolvesKilledByWeapon;
    public SO.IntVariable WolvesKilledByTrap;

    public SO.IntVariable WolvesGold;
    public SO.IntVariable EnclosureGold;
    public SO.StringVariable FavoriteEnclosure;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        RefreshVariables();
    }    

    public void RefreshVariables()
    {
        DeathCount.Set(0);
        PlayerDamageDealt.Set(0);
        GoldEarned.Set(0);
        TotalSheeps.Set(0);

        PlacedTraps.Set(0);
        TrapsDamageDealt.Set(0);
        GoldSpent.Set(0);
        FavoriteTrap.Set("None");

        Wolves.Set(0);
        Werewolves.Set(0);
        WolvesKilledByWeapon.Set(0);
        WolvesKilledByTrap.Set(0);

        WolvesGold.Set(0);
        EnclosureGold.Set(0);
        FavoriteEnclosure.Set("None");
    }

    public void SetFavoriteEnclosure()
    {
        int maxIndex = goldPerEnclosure.IndexOf(goldPerEnclosure.Max());

        if (maxIndex == 0)
            FavoriteEnclosure.Set("Close");
        else if (maxIndex == 1)
            FavoriteEnclosure.Set("Medium");
        else
            FavoriteEnclosure.Set("Far");
    }

    public void SetFavoriteTrap()
    {
        int maxIndex = goldPerTrap.IndexOf(goldPerTrap.Max());

        if (maxIndex == 0)
            FavoriteTrap.Set("Needle");
        else if (maxIndex == 1)
            FavoriteTrap.Set("Bait");
        else if (maxIndex == 2)
            FavoriteTrap.Set("Mud");
        else
            FavoriteTrap.Set("Landmine");
    }
}
