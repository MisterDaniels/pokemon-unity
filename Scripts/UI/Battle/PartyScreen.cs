using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;

namespace UI.Battle {

    public class PartyScreen : MonoBehaviour {
    

        PartyMemberUI[] memberSlots;

        public void Init() {
            memberSlots = GetComponentsInChildren<PartyMemberUI>();
        }

        public void SetPartyData(List<Pokemon> pokemons) {
            for (int i = 0; i < memberSlots.Length; i++) {
                if (i < pokemons.Count) {
                    memberSlots[i].SetData(pokemons[i]);
                } else {
                    memberSlots[i].gameObject.SetActive(false);
                }
            }
        }

    }

}