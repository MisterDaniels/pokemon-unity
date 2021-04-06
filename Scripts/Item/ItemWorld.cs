using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Monster.Characters;
using UI;

namespace Items {

    public class ItemWorld : MonoBehaviour, Interactable {

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

        public void Interact(Transform initiator) {
            List<string> texts = new List<string>();

            texts.Add($"Você quer pegar { GetItemDescription() }?");
            texts.Add("?");
            
            Dialog dialog = new Dialog();
            dialog.Lines = texts;

            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, (bool took) => {
                if (took) {
                    initiator.GetComponent<Inventory>()?.AddItem(item);
                    DestroySelf();
                }
            }));
        }

        public void SetItem(Item item) {
            this.item = item;
            spriteRenderer.sprite = item.Base.Sprite;
        }

        public void DestroySelf() {
            Destroy(gameObject);
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

    }

}