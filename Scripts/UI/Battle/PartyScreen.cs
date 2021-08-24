using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;

namespace UI.Battle {

    public class PartyScreen : MonoBehaviour {
    

        PartyMemberUI[] memberSlots;
        List<Pokemon> pokemons;

        public void Init() {
            memberSlots = GetComponentsInChildren<PartyMemberUI>();
        }

        public void SetPartyData(List<Pokemon> pokemons) {
            this.pokemons = pokemons;

            for (int i = 0; i < memberSlots.Length; i++) {
                if (i < pokemons.Count) {
                    memberSlots[i].SetData(pokemons[i]);
                } else {
                    memberSlots[i].gameObject.SetActive(false);
                }
            }
        }

        public void UpdateMemberSelection(int selectedMember) {
            for (int i = 0; i < pokemons.Count; i++) {
                if (i == selectedMember) {
                    memberSlots[i].SetSelected(true);
                } else {
                    memberSlots[i].SetSelected(false);
                }
            }
        }

    }

}