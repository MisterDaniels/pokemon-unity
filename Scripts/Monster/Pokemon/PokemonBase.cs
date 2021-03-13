using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pokemon.Moves;

namespace Pokemon {

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
        Dragon
    }

}