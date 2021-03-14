using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Pokemon.Moves;

namespace Monster.Pokemon {

    [CreateAssetMenu(fileName = "Pokémon", menuName = "Pokémon/Create", order = 0)]

    public class PokemonBase : ScriptableObject {

        [SerializeField] string name;
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Sprite frontSprite;
        [SerializeField] Sprite backSprite;
        [SerializeField] PokemonType mainType;
        [SerializeField] PokemonType subType;

        // Base Stats
        [SerializeField] int maxHp;
        [SerializeField] int attack;
        [SerializeField] int defence;
        [SerializeField] int speed;
        [SerializeField] int spAttack;
        [SerializeField] int spDefence;

        [SerializeField] List<LearnableMove> learnableMoves;

        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        public Sprite FrontSprite {
            get { return frontSprite; }
        }

        public Sprite BackSprite {
            get { return backSprite; }
        }

        public PokemonType MainType {
            get { return mainType; }
        }

        public PokemonType SubType {
            get { return subType; }
        }

        public int MaxHp {
            get { return maxHp; }
        }

        public int Attack {
            get { return attack; }
        }

        public int Defence {
            get { return defence; }
        }

        public int Speed {
            get { return speed; }
        }

        public int SpAttack {
            get { return spAttack; }
        }

        public int SpDefence {
            get { return spDefence; }
        }

        public List<LearnableMove> LearnableMoves {
            get { return learnableMoves; }
        }

    }

    [System.Serializable]
    public class LearnableMove {
        
        [SerializeField] MoveBase moveBase;
        [SerializeField] int level;

        public MoveBase Base {
            get { return moveBase; }
        }

        public int Level {
            get { return level; }
        }

    }

    public enum PokemonType {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy
    }

    public class TypeChart {

        static float[][] chart = {
            //                    NOR   FIR   WAT   ELE   GRA   ICE   FIG   POI   GRO   FLY   PSY   BUG   ROC   GHO   DRA   DAR   STE   FAI
            /*NOR*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   0.5f, 0f,   1f,   1f,   0.5f, 1f },
            /*FIR*/ new float[] { 1f,   0.5f, 0.5f, 1f,   2f,   2f,   1f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 1f,   2f,   1f },
            /*WAT*/ new float[] { 1f,   2f,   0.5f, 1f,   0.5f, 1f,   1f,   1f,   2f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f,   1f },
            /*ELE*/ new float[] { 1f,   2f,   0.5f, 0.5f, 1f,   1f,   1f,   0f,   2f,   1f,   1f,   1f,   1f,   1f,   0.5f, 1f,   1f,   1f },
            /*GRA*/ new float[] { 1f,   0.5f, 2f,   1f,   0.5f, 1f,   1f,   0.5f, 2f,   0.5f, 1f,   0.5f, 2f,   1f,   0.5f, 1f,   0.5f, 1f },
            /*ICE*/ new float[] { 1f,   0.5f, 0.5f, 1f,   2f,   0.5f, 1f,   1f,   2f,   2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f },
            /*FIG*/ new float[] { 2f,   1f,   1f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f, 0.5f, 0.5f, 2f,   0f,   1f,   2f,   2f,   0.5f },
            /*POI*/ new float[] { 1f,   1f,   1f,   1f,   2f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   1f,   0.5f, 0.5f, 1f,   1f,   0f,   2f },
            /*GRO*/ new float[] { 1f,   2f,   1f,   2f,   0.5f, 1f,   1f,   2f,   0f,   1f,   0.5f, 2f,   1f,   1f,   1f,   1f,   2f,   1f },
            /*FLY*/ new float[] { 1f,   1f,   1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   2f,   0.5f, 1f,   1f,   1f,   0.5f, 1f },
            /*PSY*/ new float[] { 1f,   1f,   1f,   1f,   1f,   1f,   2f,   2f,   1f,   1f,   0.5f, 1f,   1f,   1f,   1f,   0f,   0.5f, 1f },
            /*BUG*/ new float[] { 1f,   0.5f, 1f,   1f,   2f,   1f,   0.5f, 0.5f, 1f,   0.5f, 2f,   1f,   1f,   0.5f, 1f,   2f,   0.5f, 0.5f },
            /*ROC*/ new float[] { 1f,   2f,   1f,   1f,   1f,   2f,   0.5f, 1f,   0.5f, 2f,   1f,   2f,   1f,   1f,   1f,   1f,   0.5f, 1f },
            /*GHO*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   1f },
            /*DRA*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   2f,   1f,   0.5f, 0f },
            /*DAR*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   0.5f, 1f,   0.5f },
            /*STE*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   1f,   1f,   0.5f, 2f },
            /*FAI*/ new float[] { 0f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   1f,   2f,   1f,   1f,   2f,   2f,   2f,   0.5f, 1f }
        };

        public static float GetEffectiveness(PokemonType attackType, PokemonType defenceType) {
            if (attackType == PokemonType.None || defenceType == PokemonType.None) {
                return 1;
            }

            int row = (int) attackType - 1;
            int col = (int) defenceType - 1;

            return chart[row][col];
        }

    }

}