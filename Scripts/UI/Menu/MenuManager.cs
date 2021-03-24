using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Characters;

namespace UI.Menus {

    public enum MenuType {
        Inventory
    }

    public class MenuManager : MonoBehaviour {
        
        [SerializeField] InventoryHud inventoryHud;

        public event Action OnShowMenu;
        public event Action OnCloseMenu;

        public static MenuManager Instance { get; private set; }
        public bool IsShowingAMenu { get; private set; }
        
        private List<MenuType> showingMenus = new List<MenuType>();

        private void Awake() {
            Instance = this;
        }

        public void ApplyMenuTo(Inventory inventory) {
            inventoryHud.SetInventory(inventory);
        }

        public void ShowMenu(MenuType menuType) {
            if (CheckIfMenuIsOpened(menuType)) {
                return;
            }
            
            OnShowMenu?.Invoke();
            
            IsShowingAMenu = true;

            if (menuType == MenuType.Inventory) {
                Transform inventoryContainer = inventoryHud.gameObject.transform.Find("Inventory");
                inventoryContainer.gameObject.SetActive(true);
            }
            
            showingMenus.Add(menuType);
        }

        public void HideMenu(MenuType menuType) {
            if (!CheckIfMenuIsOpened(menuType)) {
                return;
            }
            
            if (showingMenus.Count <= 0) {
                IsShowingAMenu = false;
                OnCloseMenu?.Invoke();
            }

            if (menuType == MenuType.Inventory) {
                Transform inventoryContainer = inventoryHud.gameObject.transform.Find("Inventory");
                inventoryContainer.gameObject.SetActive(false);
            }
            
            showingMenus.Remove(menuType);
        }

        public void ShowAllMenu() {
            OnShowMenu?.Invoke();

            IsShowingAMenu = true;

            Transform inventoryContainer = inventoryHud.gameObject.transform.Find("Inventory");
            inventoryContainer.gameObject.SetActive(true);

            foreach(MenuType menuType in Enum.GetValues(typeof(MenuType))) {
                showingMenus.Add(menuType);
            }
        }

        public void HideAllMenu() {
            OnCloseMenu?.Invoke();

            IsShowingAMenu = false;

            Transform inventoryContainer = inventoryHud.gameObject.transform.Find("Inventory");
            inventoryContainer.gameObject.SetActive(false);

            showingMenus.Clear();
        }

        public bool CheckIfMenuIsOpened(MenuType menuType) {
            Debug.Log(showingMenus.Find(menu => (int) menu == (int) menuType) == menuType);
            return showingMenus.Count > 0 && 
                showingMenus.Find(menu => (int) menu == (int) menuType) == menuType;
        }

    }

}