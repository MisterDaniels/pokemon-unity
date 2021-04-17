using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Characters;
using Items;

namespace UI.Menus {

    public class InventoryHud : MonoBehaviour {

        [SerializeField] Transform itemSlotContainer;
        [SerializeField] Transform itemSlotTemplate;

        private Inventory inventory;
        private List<Transform> itemSlotTransforms = new List<Transform>(); 
        private int itemSlotQuantity = 0;

        int currentItem;
        int lastSelectedItem = -1;

        public void HandleUpdate() {
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
            if (this.inventory != inventory) {
                itemSlotQuantity = inventory.Items.Count;
                InitializeInventorySlots();
            }

            this.inventory = inventory;

            inventory.OnItemListChanged += RefreshInventoryItems;

            RefreshInventoryItems();
        }

        private void InitializeInventorySlots() {
            foreach (Transform itemSlotTransform in itemSlotTransforms) {
                Destroy(itemSlotTransform.gameObject);
            }

            itemSlotTransforms.Clear();

            for (int i = 0; i < itemSlotQuantity; i++) {
                Transform itemSlotTransform = Instantiate(itemSlotTemplate, itemSlotContainer);

                itemSlotTransform.gameObject.name = i.ToString();
                itemSlotTransform.gameObject.SetActive(true);
                itemSlotTransforms.Add(itemSlotTransform);
            }
        }

        private void RefreshInventoryItems() {
            foreach (Transform itemSlotTransform in itemSlotTransforms) {
                Image image = itemSlotTransform.Find("Image").GetComponent<Image>();
                image.gameObject.SetActive(false);

                Text quantityText = itemSlotTransform.Find("Quantity").GetComponent<Text>();
                quantityText.gameObject.SetActive(false);

                Outline outline = itemSlotTransform.GetComponent<Outline>();
                outline.effectColor = Color.black;
            }

            for (int i = 0; i < itemSlotQuantity; i++) {
                if (inventory.Items[i] != null) {
                    Outline outline = itemSlotTransforms[i].GetComponent<Outline>();
                    outline.effectColor = inventory.Items[i].Base.GetItemRarenessColor();

                    Image image = itemSlotTransforms[i].Find("Image").GetComponent<Image>();
                    image.sprite = inventory.Items[i].Base.Sprite;
                    image.gameObject.SetActive(true);

                    if (inventory.Items[i].Amount > 1) {
                        Text quantityText = itemSlotTransforms[i].Find("Quantity").GetComponent<Text>();
                        quantityText.text = inventory.Items[i].Amount.ToString();
                        quantityText.gameObject.SetActive(true);
                    }
                }
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