using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {

    public enum ItemType {
        Berry,
        Potion,
        Coin,
        Catch,
        Addon,
        Pokemon
    }

    public enum RarenessType {
        Common,
        Rare,
        Unique
    }

    public abstract class ItemBase : ScriptableObject {

        [SerializeField] string name;
        [TextArea]
        [SerializeField] string description;
        [SerializeField] Sprite sprite;

        [SerializeField] RarenessType rarenessType;

        public string Name {
            get { return name; }
        }

        public string Description {
            get { return description; }
        }

        public Sprite Sprite {
            get { return sprite; }
            set { sprite = value; }
        }

        public abstract ItemType GetType();

        public RarenessType RarenessType {
            get { return rarenessType; }
        }

        public Color GetItemRarenessColor() {
            switch (rarenessType) {
                case RarenessType.Common: {
                    return new Color32(123, 123, 123, 255);
                } case RarenessType.Rare: {
                    return new Color32(160, 64, 160, 255);
                } case RarenessType.Unique: {
                    return new Color32(224, 192, 104, 255);
                } default: {
                    return new Color32(0, 0, 0, 255);
                }
            }
        }

        public bool IsStackable() {
            switch (GetType()) {
                case ItemType.Berry:
                case ItemType.Potion:
                case ItemType.Coin:
                case ItemType.Catch:
                    return true;
                case ItemType.Addon:
                case ItemType.Pokemon:
                default:
                    return false;
            }
        }

        public virtual void Use() {
            Debug.Log("Item was used");
        }

    }

}