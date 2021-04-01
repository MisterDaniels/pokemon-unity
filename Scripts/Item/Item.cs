using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    
    [Serializable]
    public class Item {

        [SerializeField] ItemBase _base;
        [SerializeField] int amount;

        public ItemBase Base { 
            get {
                return _base;
            } 
        }
        
        public int Amount { 
            get { return amount; }
            set { amount = value; }
        }

        public Item(ItemBase itemBase, int amount) {
            this._base = itemBase;
            this.amount = amount;
        }

        public void Use() {
            _base.Use();
        }

    }

}