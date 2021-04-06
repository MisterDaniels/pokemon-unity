using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public abstract class TooltipTrigger : MonoBehaviour {
        
        protected LTDescr delay;

        protected virtual void OnMouseEnter() {
            delay = LeanTween.delayedCall(0.5f, () => {
                TooltipSystem.Show("Tooltip not implemented");
            });
        }

        protected virtual void OnMouseExit() {
            LeanTween.cancel(delay.uniqueId);
            TooltipSystem.Hide();
        }

    }

}