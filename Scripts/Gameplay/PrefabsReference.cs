using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Util {

    public class PrefabsReference : MonoBehaviour {
        
        [SerializeField] GameObject pokemonOverworld;
        [SerializeField] GameObject itemOverworld;

        public static PrefabsReference Instance { get; set; }

        private void Awake() {
            Instance = this;
        }

        public GameObject PokemonOverworld {
            get => pokemonOverworld;
        }

        public GameObject ItemOverworld {
            get => itemOverworld;
        }

    }

}