using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {

    [CreateAssetMenu(fileName = "Item", menuName = "Item/Create", order = 0)]

    public class ItemBase : ScriptableObject {

        [SerializeField] string name;
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Sprite sprite;

        [SerializeField] ItemType itemType;
        [SerializeField] RarenessType rarenessType;

        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        public Sprite Sprite {
            get { return sprite; }
        }

        public ItemType ItemType {
            get { return ItemType; }
        }

        public RarenessType RarenessType {
            get { return rarenessType; }
        }

        public Color GetItemRarenessColor() {
            switch (rarenessType) {
                case RarenessType.Common: {
                    return new Color(1f, 1f, 1f, 1f);
                } case RarenessType.Rare: {
                    return new Color(1f, 1f, 1f, 1f);
                } case RarenessType.Unique: {
                    return new Color(1f, 1f, 1f, 1f);
                } default: {
                    return new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        public bool IsStackable() {
            switch (itemType) {
                case ItemType.Berry:
                case ItemType.Potion:
                case ItemType.Coin:
                case ItemType.Catch:
                    return true;
                case ItemType.Addon:
                default:
                    return false;
            }
        }

    }

    public enum ItemType {
        Berry,
        Potion,
        Coin,
        Catch,
        Addon
    }

    public enum RarenessType {
        Common,
        Rare,
        Unique
    }

}