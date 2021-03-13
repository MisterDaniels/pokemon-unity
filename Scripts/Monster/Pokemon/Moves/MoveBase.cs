using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Moves {
    
    [CreateAssetMenu(fileName = "Move", menuName = "Pokémon/Move/Create", order = 0)]

    public class MoveBase : ScriptableObject {
        
        [SerializeField] string name;
        [SerializeField] string description;
        [SerializeField] PokemonType type;
        [SerializeField] int power;
        [SerializeField] int accuracy;
        [SerializeField] int pp;
        
        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        public PokemonType Type {
            get { return type; }
        }

        public int Power {
            get { return power; }
        }

        public int Accuracy {
            get { return accuracy; }
        }

        public int PP {
            get { return pp; }
        }

    }

}