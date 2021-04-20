using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UI;
using Util;
using Map;
using Items;
using Core;
using Monster.Characters;

namespace Core.Mechanic {

    public class DragDrop : GameUtils, IBeginDragHandler, IEndDragHandler, IDragHandler {

        [SerializeField] public float maxDistance = 0f;

        public bool IsDragging { get; private set; }

        private GameObject itemHighlight;

        public GameObject ItemHighlight => itemHighlight;

        private bool isSlot = false;

        public void OnBeginDrag(PointerEventData eventData) {
            float distance = 0f;
            if (maxDistance != 0f) {
                distance = Vector3.Distance(GameController.Instance.PlayerController.transform.position,
                    transform.position);
            }
            
            if (maxDistance == 0f || distance <= maxDistance) {
                if (isSlot) {
                    Item itemInSlot = GetComponent<ItemSlot>()?.GetItem();

                    if (itemInSlot == null) return;
                }

                IsDragging = true;

                TooltipSystem.Hide();

                itemHighlight = Instantiate(PrefabsReference.Instance.ItemCanved, 
                    GameController.Instance.Canvas.transform);

                Image itemHighlightImage = itemHighlight.GetComponent<Image>();
                Item itemDragging = GetComponent<Draggable>().GetItem();

                itemHighlightImage.sprite = itemDragging.Base.Sprite;
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (IsDragging) {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - itemHighlight.transform.position;

                itemHighlight.transform.Translate(mousePosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (IsDragging) {
                IsDragging = false;

                GameObject draggedGameObject = eventData.pointerCurrentRaycast.gameObject;
                
                if (!draggedGameObject) {
                    if (isSlot) {
                        ItemSlot itemSlot = GetComponent<ItemSlot>();

                        ItemWorld droppedItemWorld = Instantiate(PrefabsReference.Instance.ItemOverworld).GetComponent<ItemWorld>();

                        droppedItemWorld.SetItem(itemSlot.GetItem());
                        droppedItemWorld.SetObjectPositionAndSnapToTile(itemHighlight.transform.position);

                        GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.RemoveAllItem(int.Parse(gameObject.name));
                        itemSlot.SetItem(null);
                    } else {
                        SetObjectPositionAndSnapToTile(itemHighlight.transform.position);
                    }
                }

                if (draggedGameObject && GameLayers.i.DraggableLayers == (GameLayers.i.DraggableLayers | (1 << draggedGameObject.layer))) {
                    if (draggedGameObject.GetComponent<SpriteRenderer>().sortingLayerName != GameLayers.i.InterfaceSortingLayerName) {
                        SetObjectPositionAndSnapToTile(itemHighlight.transform.position);
                    }
                }

                Destroy(itemHighlight.gameObject);
            }
        }

        private void Awake() {
            isSlot = GetComponent<ItemSlot>() ? true : false;
        }

    }

}