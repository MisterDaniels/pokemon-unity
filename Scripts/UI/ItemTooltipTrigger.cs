using System.Collections;
using UnityEngine;
using Items;
using Core.Mechanic;

namespace UI {

    public class ItemTooltipTrigger : TooltipTrigger {
    
        private ItemWorld itemWorld;

        private void Awake() {
            itemWorld = GetComponent<ItemWorld>();
        }

        protected override void OnMouseEnter() {
            if (!GetComponent<DragDrop>().IsDragging) {
                delay = LeanTween.delayedCall(0.5f, () => {
                    TooltipSystem.Show(itemWorld.GetItemDescription());
                });
            }
        }

        protected override void OnMouseExit() {
            if (delay != null) {
                LeanTween.cancel(delay.uniqueId);
                TooltipSystem.Hide();
            }
        }

    }

}