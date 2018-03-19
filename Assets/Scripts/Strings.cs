using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Strings
{
    public static readonly Dictionary<string, string> Description = new Dictionary<string, string>()
    {
        {"PlayerDamageDealt",  "Damage dealt by the player"},
        {"GoldEarned",  "Total gold earned"},
        {"TotalSheeps",  "Total sheeps"},

        {"PlacedTraps",  "Total traps placed"},
        {"TrapsDamageDealt",  "Total damage dealt by the traps"},
        {"GoldSpent",  "Gold spent in traps"},
        {"FavoriteTrap",  "Favorite trap"},

        {"Wolves",  "Wolves killed"},
        {"Werewolves",  "Werewolves killed"},
        {"WolvesKilledByWeapon",  "Wolves killed by weapon"},
        {"WolvesKilledByTrap",  "Wolves killed by trap"},

        {"WolvesGold",  "Gold earned by killing wolves"},
        {"EnclosureGold",  "Gold earned by enclosures"},
        {"FavoriteEnclosure",  "Favorite enclosure"}
    };
}
