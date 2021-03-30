using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Battle;
using Monster.Creature;

namespace UI {

    public class PokemonSlotHud : MonoBehaviour {

        [SerializeField] Image pokemonIconImage;
        [SerializeField] Text pokemonNameText;
        [SerializeField] HPBar pokemonHpBar;

        public void AttachPokemon(Pokemon pokemon) {
            pokemonIconImage.sprite = pokemon.Base.IconSprite;
            pokemonNameText.text = $"{ pokemon.Base.Name } ({ pokemon.Level })";
            pokemonHpBar.SetHPSmooth(pokemon.HP);
        }

    }

}