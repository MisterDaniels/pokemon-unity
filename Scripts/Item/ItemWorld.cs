using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Monster.Characters;
using UI;
using Util;

namespace Items {

    public class ItemWorld : GameUtils, Draggable {

        [SerializeField] Item item;
        private SpriteRenderer spriteRenderer;
        private DragDrop dragDrop;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            dragDrop = GetComponent<DragDrop>();
            
            if (item.Base != null) {
                SetItem(item);
            }
        }

        public void SetItem(Item item) {
            this.item = item;
            spriteRenderer.sprite = item.Base.Sprite;
        }

        public void DestroySelf() {
            Destroy(gameObject);
            Destroy(dragDrop.ItemHighlight);
        }

        public string GetItemDescription() {
            switch(item.Base.GetType()) {
                case ItemType.Pokemon:
                    return $"{ item.Base.Name } com o Pokémon <color=#{ ColorUtility.ToHtmlStringRGBA(((Pokeball) item.Base).Pokemon.Base.GetPokemonMainTypeColor()) }>{ ((Pokeball) item.Base).Pokemon.Base.Name }</color> <sprite name=\"{ ((Pokeball) item.Base).Pokemon.Base.Name }\"> level { ((Pokeball) item.Base).Pokemon.Level }";
                    break;
                case ItemType.Catch:
                case ItemType.Potion:
                default:
                    return $"<color=#{ ColorUtility.ToHtmlStringRGBA(item.Base.GetItemRarenessColor()) }>{ item.Base.Name }</color> <sprite name=\"{ item.Base.Name }\"> { item.Amount }x";
                    break;
            }
        }

        public Item GetItem() {
            return item;
        }

    }

}