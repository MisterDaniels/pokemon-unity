using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Monster.Creature;
using Core;

namespace Monster.Characters {

    public class Inventory : MonoBehaviour {

        [SerializeField] Dictionary<int, Item> items;
        [SerializeField] int size;

        public event Action OnItemListChanged;

        public Dictionary<int, Item> Items { 
            get {
                return items;
            }
        }

        public int Size { 
            get {
                return size;
            }
            set {
                size = value;
            }
        }

        public void AddItem(Item item, int slotIndex = -1, Action<bool, Item> OnAddOver = null) {
            bool itemAdded = false;
            Item itemToDrop = null;
            
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
                    break;
            }

            if (slotIndex == -1) {
                slotIndex = GetFirstIndexItemsSlotAvaiable();
            }

            if (items[slotIndex] != null) {
                itemToDrop = items[slotIndex];
            }

            if (item.Base.IsStackable()) {
                bool itemAlreadyInInventory = false;

                if (items[slotIndex] != null && 
                    items[slotIndex].Base.Name == item.Base.Name) {
                    items[slotIndex].Amount += item.Amount;
                    itemAlreadyInInventory = true;
                    itemAdded = true;
                    itemToDrop = null;
                }

                if (!itemAlreadyInInventory) {
                    items[slotIndex] = item;
                    itemAdded = true;
                }
            } else {
                items[slotIndex] = item;
                itemAdded = true;
            }

            OnAddOver?.Invoke(itemAdded, itemToDrop);
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
                items.Remove(index);
            }

            OnItemListChanged?.Invoke();

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

        private void Awake() {
            items = new Dictionary<int, Item>(size);
            
            for (int i = 0; i < size; i++) {
                items.Add(i, null);
            }
        }

        private int GetFirstIndexItemsSlotAvaiable() {
            for (int i = 0; i < items.Count; i++) {
                if (items[i] == null) {
                    return i;
                }
            }

            return -1;
        }

    }

}