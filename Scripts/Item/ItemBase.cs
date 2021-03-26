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