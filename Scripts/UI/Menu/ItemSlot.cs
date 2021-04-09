using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Items;
using Core;
using Monster.Characters;

public class ItemSlot : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        ItemWorld itemWorld = eventData.pointerDrag.GetComponent<ItemWorld>();

        GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>()?.AddItem(itemWorld.Item);

        itemWorld.DestroySelf();
    }

}