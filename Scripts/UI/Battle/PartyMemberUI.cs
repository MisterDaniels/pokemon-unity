using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;

namespace UI.Battle {

    public class PartyMemberUI : MonoBehaviour {
        
        [SerializeField] Text nameText;
        [SerializeField] HPBar hpBar;
        [SerializeField] Image imageIcon;

        Pokemon _pokemon;

        public void SetData(Pokemon pokemon) {
            _pokemon = pokemon;

            nameText.text = $"{_pokemon.Base.Name} ({_pokemon.Level})";
            hpBar.SetHP((float) _pokemon.HP / _pokemon.MaxHp);

            imageIcon.sprite = _pokemon.Base.IconSprite;
        }        

    }

}