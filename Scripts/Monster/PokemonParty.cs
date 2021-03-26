using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Monster.Creature {
    
    public class PokemonParty : MonoBehaviour {

        [SerializeField] List<Pokemon> pokemons;

        private void Start() {
            RefreshPokemons();
        }

        public Pokemon GetHealthyPokemon() {
            return pokemons.Where(pokemon => pokemon.HP > 0).FirstOrDefault();
        }

        public bool IsPartyComplete() {
            return pokemons.Count == 6;
        }

        public void AddPokemon(Pokemon pokemon) {
            pokemons.Add(pokemon);
            RefreshPokemons();
        }

        public void RemovePokemon(int index) {
            RefreshPokemons();
        }

        public void RefreshPokemons() {
            foreach (var pokemon in pokemons) {
                pokemon.Init();
            }
        }

        public void Load() {
            //this = GameAssets.i.LoadPokemonParty(0);
        }

        public void Save() {
            //GameAssets.i.Save();
        }

    }

}