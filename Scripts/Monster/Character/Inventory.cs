using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Monster.Creature;
using Core;

namespace Monster.Characters {

    public class Inventory : MonoBehaviour {

        [SerializeField] List<Item> items;

        public event Action OnItemListChanged;

        public List<Item> Items { 
            get {
                return items;
            }
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

        public bool RemoveItem(int index, int amount) {
            Item itemToRemove = items[index];

            if (itemToRemove.Amount - amount < 0) {
                Debug.Log("Está tentando remover uma quantidade insuficiente presente no inventário");
                return false;
            }
            
            itemToRemove.Amount -= amount;

            if (itemToRemove.Amount == 0) {
                items.RemoveAt(index);
            }

            return true;
        }

        public void DropItem(int index, int amount) {
            Item itemToDrop = items[index];

            bool removedItem = RemoveItem(index, amount);

            if (removedItem) {
                itemToDrop.Amount = amount;

                SpawnManager.Instance.SpawnItemInWorld(itemToDrop,
                    new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f));
            }
        }

    }

}