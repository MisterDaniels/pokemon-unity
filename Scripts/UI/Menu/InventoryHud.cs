using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Characters;
using Items;

namespace UI.Menus {

    public class InventoryHud : MonoBehaviour {

        private Inventory inventory;
        private Transform itemSlotContainer;
        private Transform itemSlotTemplate;

        private void Awake() {
            itemSlotContainer = gameObject.transform.Find("Inventory");
            itemSlotTemplate = itemSlotContainer.Find("ItemSlot");
        }

        public void SetInventory(Inventory inventory) {
            this.inventory = inventory;

            inventory.OnItemListChanged += RefreshInventoryItems;

            RefreshInventoryItems();
        }

        private void RefreshInventoryItems() {
            foreach (Transform child in itemSlotContainer) {
                if (child == itemSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach(Item item in inventory.Items) {
                Transform itemSlotTransform = Instantiate(itemSlotTemplate, itemSlotContainer);
                
                Outline outline = itemSlotTransform.GetComponent<Outline>();
                outline.effectColor = item.Base.GetItemRarenessColor();

                Image image = itemSlotTransform.Find("Image").GetComponent<Image>();
                image.sprite = item.Base.Sprite;

                if (item.Amount > 1) {
                    Text quantityText = itemSlotTransform.Find("Quantity").GetComponent<Text>();
                    quantityText.text = item.Amount.ToString();
                    quantityText.gameObject.SetActive(true);
                }

                itemSlotTransform.gameObject.SetActive(true);
            }
        }

    }

}