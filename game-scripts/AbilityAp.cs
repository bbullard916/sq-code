using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAp : MonoBehaviour
{
    Dictionary<string, int> abilities = new Dictionary<string, int>()
    {
        // Warrior
        { "Basic Attack", 5 },
        { "Lacerate", 8 },
        { "Heal", 5 },
        { "Lightning Strike", 12 },
        { "Shield", 7 }

        // Mage

        // Ranger
    };

    public int GetAbilityValue(string abilityName)
    {
        if (abilities.TryGetValue(abilityName, out int value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning($"Ability '{abilityName}' not found.");
            return -1; // or throw an exception, or return 0, depending on your needs
        }
    }
}
