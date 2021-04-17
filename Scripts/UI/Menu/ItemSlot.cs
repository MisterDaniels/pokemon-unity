using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Items;
using Core;
using Monster.Characters;
using Util;
using Core.Mechanic;

namespace UI {

    public class ItemSlot : GameUtils, IDropHandler, Draggable {

        private Item item;

        public void OnDrop(PointerEventData eventData) {
            Item itemDropped = eventData.pointerDrag.GetComponent<Draggable>().GetItem();

            item = itemDropped;

            GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.AddItem(itemDropped, 
                int.Parse(gameObject.name), (bool added, Item itemToDrop) => {
                    ItemWorld itemWorld = eventData.pointerDrag.GetComponent<ItemWorld>();
                    ItemSlot itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();

                    if (itemToDrop != null) {
                        if (itemSlot != null) {
                            if (itemSlot.GetItem() != null) {
                                GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.AddItem(itemToDrop,
                                    int.Parse(itemSlot.gameObject.name));
                            }
                        } else {
                            GameObject droppedItemWorld = Instantiate(PrefabsReference.Instance.ItemOverworld,
                                itemWorld.transform.position, Quaternion.identity);
                            droppedItemWorld.GetComponent<ItemWorld>().SetItem(itemToDrop);
                        }
                    }

                    if (added && itemSlot == null) {
                        itemWorld.DestroySelf();
                    }
                });
        }

        public Item GetItem() {
            return item;
        }

    }

}