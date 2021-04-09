using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UI;
using Util;
using Map;
using Items;

namespace Core.Mechanic {

    public class DragDrop : GameUtils, IBeginDragHandler, IEndDragHandler, IDragHandler {

        public bool IsDragging { get; private set; }

        private GameObject itemHighlight;

        public GameObject ItemHighlight => itemHighlight;

        public void OnBeginDrag(PointerEventData eventData) {
            IsDragging = true;

            TooltipSystem.Hide();

            itemHighlight = Instantiate(PrefabsReference.Instance.ItemCanved, 
                GameController.Instance.Canvas.transform);

            Image itemHighlightImage = itemHighlight.GetComponent<Image>();
            ItemWorld itemWorld = GetComponent<ItemWorld>();

            itemHighlightImage.sprite = itemWorld.Item.Base.Sprite;
        }

        public void OnDrag(PointerEventData eventData) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - itemHighlight.transform.position;

            itemHighlight.transform.Translate(mousePosition);
        }

        public void OnEndDrag(PointerEventData eventData) {
            IsDragging = false;

            GameObject draggedGameObject = eventData.pointerCurrentRaycast.gameObject;
            
            if (!draggedGameObject) {
                SetPositionAndSnapToTile(itemHighlight.transform.position);
            }

            if (draggedGameObject && GameLayers.i.DraggableLayers == (GameLayers.i.DraggableLayers | (1 << draggedGameObject.layer))) {
                if (draggedGameObject.layer != GameLayers.i.InterfaceLayer) {
                    SetPositionAndSnapToTile(itemHighlight.transform.position);
                }
            }

            Destroy(itemHighlight.gameObject);
        }

    }

}