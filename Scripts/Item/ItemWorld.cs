using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {

    public class ItemWorld : MonoBehaviour {

        [SerializeField] Item item;
        private SpriteRenderer spriteRenderer;

        public Item Item {
            get { return item; }
        }

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
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
        }

    }

}