using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monster.Creature {
    
    public class PokemonParty : MonoBehaviour {

        [SerializeField] List<Pokemon> pokemons;

        private void Start() {
            foreach (var pokemon in pokemons) {
                pokemon.Init();
            }
        }

        public Pokemon GetHealthyPokemon() {
            return pokemons.Where(pokemon => pokemon.HP > 0).FirstOrDefault();
        }

    }

}