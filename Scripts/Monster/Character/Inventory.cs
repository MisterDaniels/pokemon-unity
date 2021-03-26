using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Monster.Creature;

namespace Monster.Characters {

    public class Inventory : MonoBehaviour {

        [SerializeField] List<Item> items;

        public event Action OnItemListChanged;

        public List<Item> Items { 
            get {
                return items;
            }
        }

        private void Start() {
            
        }

        public void AddItem(Item item) {
            switch(item.Base.GetType()) {
                case ItemType.Pokemon:
                    PokemonParty pokemonParty = GetComponent<PokemonParty>();
                    
                    if (pokemonParty == null) {
                        return;
                    }

                    if (!pokemonParty.IsPartyComplete()) {
                        pokemonParty.AddPokemon(((Pokeball) item.Base).Pokemon);
                        item.Base.Sprite = ((Pokeball) item.Base).Pokemon.Base.IconSprite;
                    }

                    break;
                default:
                    Debug.Log("table");
                    break;
            }

            if (item.Base.IsStackable()) {
                bool itemAlreadyInInventory = false;

                foreach (Item inventoryItem in items) {
                    if (inventoryItem.Base.Name == item.Base.Name) {
                        inventoryItem.Amount += item.Amount;
                        itemAlreadyInInventory = true;
                    }
                }

                if (!itemAlreadyInInventory) {
                    items.Add(item);
                }
            } else {
                items.Add(item);
            }

            OnItemListChanged?.Invoke();
        }

        public void RemoveItem() {

        }

    }

}