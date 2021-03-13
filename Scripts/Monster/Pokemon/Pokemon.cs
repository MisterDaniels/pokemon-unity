using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.Moves;

namespace Pokemon {

    public class Pokemon {

        public PokemonBase Base { get; set };
        public int Level { get; set };
        public int HP { get; set; }
        public List<Move> Moves { get; set; }

        public Pokemon(PokemonBase pBase, int pLevel) {
            Base = pBase;
            Level = pLevel;
            HP = _base.MaxHp;

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
            get { return Mathf.FloorToInt((_base.MaxHp * level) / 100f) + 10; }
        }

        public int Attack {
            // Pokémon original formula
            get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
        }

        public int Defence {
            // Pokémon original formula
            get { return Mathf.FloorToInt((_base.Defence * level) / 100f) + 5; }
        }

        public int Speed {
            // Pokémon original formula
            get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
        }

        public int SpAttack {
            // Pokémon original formula
            get { return Mathf.FloorToInt((_base.SpAttack * level) / 100f) + 5; }
        }

        public int SpDefence {
            // Pokémon original formula
            get { return Mathf.FloorToInt((_base.SpDefence * level) / 100f) + 5; }
        }

    }

}