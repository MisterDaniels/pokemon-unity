using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Characters;
using Items;

namespace UI.Menus {

    public class InventoryHud : MonoBehaviour {

        private Inventory inventory;
        [SerializeField] Transform itemSlotContainer;
        [SerializeField] Transform itemSlotTemplate;

        private List<Transform> itemSlotTransforms = new List<Transform>(); 

        int currentItem;
        int lastSelectedItem = -1;

        public void HandleUpdate() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentItem < inventory.Items.Count - 1) {
                    ++currentItem;
                }
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (currentItem > 0) {
                    --currentItem;
                }
            }

            if (itemSlotTransforms.Count > 0) {
                UpdateItemSelection(currentItem);

                if (Input.GetKeyDown(KeyCode.X)) { // Drop item
                    inventory.DropItem(currentItem, 1);
                    RefreshInventoryItems();
                } else if (Input.GetKeyDown(KeyCode.Z)) { // Use item
                    inventory.RemoveItem(currentItem, 1);
                    inventory.Items[currentItem].Use();
                    RefreshInventoryItems();
                }
            }
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

            itemSlotTransforms.Clear();

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
                itemSlotTransforms.Add(itemSlotTransform);
            }
        }

        private void UpdateItemSelection(int selectedItem) {
            Image image = itemSlotTransforms[selectedItem].GetComponent<Image>();
            image.color = DialogManager.Instance.HightlightedColor;

            if (lastSelectedItem >= 0) {
                image = itemSlotTransforms[lastSelectedItem].GetComponent<Image>();
                image.color = Color.white;
            }

            lastSelectedItem = selectedItem;
        }

    }

}