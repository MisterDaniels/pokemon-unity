using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Util {

    public class PrefabsReference : MonoBehaviour {
        
        [SerializeField] GameObject pokemonOverworld;

        public static PrefabsReference i { get; set; }

        private void Awake() {
            i = this;
        }

        public GameObject PokemonOverworld {
            get => pokemonOverworld;
        }

    }

}