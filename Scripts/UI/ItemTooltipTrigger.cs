using System.Collections;
using UnityEngine;
using Items;

namespace UI {

    public class ItemTooltipTrigger : TooltipTrigger {
    
        private ItemWorld itemWorld;

        private void Awake() {
            itemWorld = GetComponent<ItemWorld>();
        }

        protected override void OnMouseEnter() {
            delay = LeanTween.delayedCall(0.1f, () => {
                TooltipSystem.Show(itemWorld.GetItemDescription());
            });
        }

        protected override void OnMouseExit() {
            LeanTween.cancel(delay.uniqueId);
            TooltipSystem.Hide();
        }

    }

}