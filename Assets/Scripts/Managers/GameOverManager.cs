using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    public SO.StringVariable DeathCount;
    public SO.StringVariable PlayerDamageDealt;
    public SO.StringVariable GoldEarned;
    public SO.StringVariable TotalSheeps;

    public SO.StringVariable PlacedTraps;
    public SO.StringVariable TrapsDamageDealt;
    public SO.StringVariable GoldSpent;
    public SO.StringVariable FavoriteTrap;

    public SO.StringVariable Wolves;
    public SO.StringVariable Werewolves;
    public SO.StringVariable WolvesKilledByWeapon;
    public SO.StringVariable WolvesKilledByTrap;

    public SO.StringVariable WolvesGold;
    public SO.StringVariable EnclosureGold;
    public SO.StringVariable FavoriteEnclosure;

    int a = 0;
    private void OnEnable()
    {
        DeathCount.Set(a++);
    }
}
