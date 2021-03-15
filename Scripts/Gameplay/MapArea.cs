using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Map {

    public class MapArea : MonoBehaviour {
        
        [SerializeField] List<Pokemon> wildPokemons;

        public Pokemon GetRandomWildPokemon() {
            var wildPokemon = wildPokemons[Random.Range(0, wildPokemons.Count)];
            wildPokemon.Init();

            return wildPokemon;
        }

    }

}