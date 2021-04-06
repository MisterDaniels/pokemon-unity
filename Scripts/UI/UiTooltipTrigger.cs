using System.Collections;
using UnityEngine;

namespace UI {

    public class UiTooltipTrigger : TooltipTrigger {
    
        
        [Multiline()]
        [SerializeField] public string text;

        protected override void OnMouseEnter() {
            delay = LeanTween.delayedCall(0.1f, () => {
                TooltipSystem.Show(text);
            });
        }

        protected override void OnMouseExit() {
            LeanTween.cancel(delay.uniqueId);
            TooltipSystem.Hide();
        }

    }

}