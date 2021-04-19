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

            if (itemDropped != null) {
                ItemSlot itemSlot = eventData.pointerDrag.GetComponent<ItemSlot>();

                if (itemSlot != null) {
                    if (itemSlot.GetItem() != null) {
                        if (item != null) {
                            GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.ChangeItemOrder(
                                int.Parse(itemSlot.gameObject.name), int.Parse(gameObject.name));
                            itemSlot.SetItem(item);
                        } else {
                            GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.AddItem(itemDropped,
                                int.Parse(gameObject.name));
                            GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.RemoveAllItem(int.Parse(itemSlot.gameObject.name));
                            itemSlot.SetItem(null);
                        }
                    }
                } else {
                    GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.AddItem(itemDropped, 
                        int.Parse(gameObject.name), (bool added, Item itemToDrop) => {
                            ItemWorld itemWorld = eventData.pointerDrag.GetComponent<ItemWorld>();

                            if (itemToDrop != null) {
                                GameObject droppedItemWorld = Instantiate(PrefabsReference.Instance.ItemOverworld,
                                    itemWorld.transform.position, Quaternion.identity);
                                droppedItemWorld.GetComponent<ItemWorld>().SetItem(itemToDrop);
                            }

                            if (added) {
                                itemWorld.DestroySelf();
                            }
                        });
                }

                item = itemDropped;
            }
        }

        public Item GetItem() {
            return item;
        }

        public void SetItem(Item item) {
            this.item = item;
        }

    }

}