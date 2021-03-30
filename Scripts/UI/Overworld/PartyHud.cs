using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace UI {

    public class PartyHud : MonoBehaviour {

        [SerializeField] List<PokemonSlotHud> pokemonsSlotHud;

        public void RefreshPokemons(PokemonParty pokemonParty) {
            foreach (PokemonSlotHud pokemonSlotHud in pokemonsSlotHud) {
                pokemonSlotHud.gameObject.SetActive(false);
            }

            for(int i = 0; i < pokemonParty.Pokemons.Count; i++) {
                pokemonsSlotHud[i].gameObject.SetActive(true);
                pokemonsSlotHud[i].AttachPokemon(pokemonParty.Pokemons[i]);
            }
        }

    }

}