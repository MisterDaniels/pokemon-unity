using System.Collections;
using UnityEngine;

namespace UI {

    public class TooltipTrigger : MonoBehaviour {

        [Multiline()]
        [SerializeField] public string text;

        private static LTDescr delay;
    
        private void OnMouseEnter() {
            delay = LeanTween.delayedCall(0.1f, () => {
                TooltipSystem.Show("teste qualquer");
            });
        }

        private void OnMouseExit() {
            LeanTween.cancel(delay.uniqueId);
            TooltipSystem.Hide();
        }

    }

}