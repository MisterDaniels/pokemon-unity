using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    
    [CreateAssetMenu(fileName = "Potion", menuName = "Item/Potion/Create", order = 0)]
    public class Potion : ItemBase {

        public override ItemType GetType() {
            return ItemType.Potion;
        }

        public override void Use() {
            Debug.Log("Potion was used");
        }

    }

}