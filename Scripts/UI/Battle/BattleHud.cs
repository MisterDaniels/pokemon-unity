using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Pokemon;

namespace UI.Battle {

    public class BattleHud : MonoBehaviour {

        [SerializeField] Text nameText;
        [SerializeField] Text levelText;
        [SerializeField] HPBar hpBar;

        Pokemon _pokemon;

        public void SetData(Pokemon pokemon) {
            _pokemon = pokemon;

            nameText.text = _pokemon.Base.Name;
            levelText.text = "Lv. " + _pokemon.Level;
            hpBar.SetHP((float) _pokemon.HP / _pokemon.MaxHp);
        }

        public IEnumerator UpdateHP() {
            yield return hpBar.SetHPSmooth((float) _pokemon.HP / _pokemon.MaxHp);
        }

    }

}