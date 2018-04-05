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

    public static readonly Dictionary<string, string> IngameInterface = new Dictionary<string, string>()
    {
        {"PassDay", " commence \n appuyez sur n pour passer au tour suivant " },
        {"Round"," tour " },
        {"Gold", " or "},
        {"SheepsInInventory", " moutons dans l'inventaire "},
        {"SheepsInInventoryAnd", " moutons dans l'inventaire et "},
        {"SheepsEaten", " Un mouton a été dévoré "},
        {"ActivatedCheats", " cheats activés "},
        {"NightEndCondition", " La nuit ne passera que lorsque tous les loups seront morts "},
        {"NotEnoughGold", " Pas assez d'or "},
        {"SuperSheeps", " super moutons "},
    };
    public static readonly Dictionary<string, string> TrapPanel = new Dictionary<string, string>()
    {
        {"Level", " Niveau " },

    };
    public static readonly Dictionary<string, string> Menu = new Dictionary<string, string>()
    {
        {"Hi", " Salut " },
        {"UnknownPlayer" , "JoueurAnonyme"}
    };
    
}
