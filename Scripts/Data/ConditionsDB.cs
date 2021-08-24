using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Monster.Creature.Data {

    public class ConditionsDB {

        public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>() {
            {
                ConditionID.psn,
                new Condition() {
                    Name = "Poison",
                    StartMessage = "has been poisoned",
                    OnAfterTurn = (Pokemon pokemon) => {
                        pokemon.UpdateHP(pokemon.MaxHp / 8);
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to poison");
                    }
                }
            },
            {
                ConditionID.brn,
                new Condition() {
                    Name = "Burn",
                    StartMessage = "has been burned",
                    OnAfterTurn = (Pokemon pokemon) => {
                        pokemon.UpdateHP(pokemon.MaxHp / 16);
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to burn");
                    }
                }
            },
            {
                ConditionID.par,
                new Condition() {
                    Name = "Paralyzed",
                    StartMessage = "has been paralyzed",
                    OnBeforeMove = (Pokemon pokemon) => {
                        if (Random.Range(1, 5) == 1) {
                            pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s paralyzed and can't move");
                            return false;
                        }
                        
                        return true;
                    }
                }
            },
            {
                ConditionID.frz,
                new Condition() {
                    Name = "Freeze",
                    StartMessage = "has been frozen",
                    OnBeforeMove = (Pokemon pokemon) => {
                        if (Random.Range(1, 5) == 1) {
                            pokemon.CureStatus();
                            pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name}'s is not frozen anymore");
                            return true;
                        }
                        
                        return false;
                    }
                }
            },
            {
                ConditionID.slp,
                new Condition() {
                    Name = "Sleep",
                    StartMessage = "has fallen asleep",
                    OnStart = (Pokemon pokemon) => {
                        // SLeep for 1-3 turns
                        pokemon.StatusTime = Random.Range(1, 4);
                        Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");   
                    },
                    OnBeforeMove = (Pokemon pokemon) => {
                        if (pokemon.StatusTime <= 0) {
                            pokemon.CureStatus();
                            pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} wake up!");
                            return true;
                        }

                        pokemon.StatusTime--;
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping");
                        return false;
                    }
                }
            }
        };

    }

    public enum ConditionID {
        none,
        psn,
        brn,
        slp,
        par,
        frz
    }

}