using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature.Moves;

namespace Monster.Creature {

    [System.Serializable]
    public class Pokemon {

        [SerializeField] PokemonBase _base;
        [SerializeField] int level;

        public PokemonBase Base { 
            get {
                return _base;
            } 
        }
        public int Level { 
            get {
                return level;
            } 
        }

        public int HP { get; set; }
        public List<Move> Moves { get; set; }

        public void Init() {
            HP = MaxHp;

            Moves = new List<Move>();
            foreach (var move in Base.LearnableMoves) {
                if (move.Level <= Level) {
                    Moves.Add(new Move(move.Base));
                }

                if (Moves.Count == 4) {
                    break;
                }
            }
        }

        public int MaxHp {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
        }

        public int Attack {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
        }

        public int Defence {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.Defence * Level) / 100f) + 5; }
        }

        public int Speed {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
        }

        public int SpAttack {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
        }

        public int SpDefence {
            // Pokémon original formula
            get { return Mathf.FloorToInt((Base.SpDefence * Level) / 100f) + 5; }
        }

        public DamageDetails TakeDamage(Move move, Pokemon attacker) {
            float critical = 1f;
            if (Random.value * 100f <= 6.25f) {
                critical = 2f;
            }

            // Pokémon original formula
            float TypeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, Base.MainType) * TypeChart.GetEffectiveness(move.Base.Type, Base.SubType);

            var DamageDetails = new DamageDetails() {
                TypeEffectiveness = TypeEffectiveness,
                Critical = critical,
                Fainted = false
            };

            float attack = (move.Base.IsSpecial) ? attacker.SpAttack : attacker.Attack;
            float defence = (move.Base.IsSpecial) ? SpDefence : Defence;

            float modifiers = Random.Range(0.85f, 1f) * TypeEffectiveness * critical;
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * move.Base.Power * ((float) attack / defence ) + 2;
            int damage = Mathf.FloorToInt(d * modifiers);

            HP -= damage;
            
            if (HP <= 0) {
                HP = 0;
                DamageDetails.Fainted = true;
            }

            return DamageDetails;
        }

        public Move GetRandomMove() {
            int r = Random.Range(0, Moves.Count);

            return Moves[r];
        }

    }

    public class DamageDetails {

        public bool Fainted { get; set; }
        public float Critical { get; set; }
        public float TypeEffectiveness { get; set; }

    }

}