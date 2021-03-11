using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon {

    public class Pokemon {

        public PokemonBase Base { get; set };
        public int Level { get; set };
        public int HP { get; set; }
        public List<Move> Moves { get; set; }

        public Pokemon(PokemonBase pBase, int pLevel) {
            Base = pBase;
            Level = pLevel;
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
            get { return Math.FloorToInt((Base.MaxHp * Level) / 100f) + 10; }
        }

        public int Attack {
            // Pokémon original formula
            get { return Math.FloorToInt((Base.Attack * Level) / 100f) + 5; }
        }

        public int Defence {
            // Pokémon original formula
            get { return Math.FloorToInt((Base.Defence * Level) / 100f) + 5; }
        }

        public int Speed {
            // Pokémon original formula
            get { return Math.FloorToInt((Base.Speed * Level) / 100f) + 5; }
        }

        public int SpAttack {
            // Pokémon original formula
            get { return Math.FloorToInt((Base.SpAttack * Level) / 100f) + 5; }
        }

        public int SpDefence {
            // Pokémon original formula
            get { return Math.FloorToInt((Base.SpDefence * Level) / 100f) + 5; }
        }

    }

}