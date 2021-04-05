using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {

    public class TooltipSystem : MonoBehaviour {

        public Tooltip tooltip;

        private static TooltipSystem instance;

        public void Awake() {
            instance = this;
        }

        public static void Show(string text) {
            instance.tooltip.SetText(text);
            instance.tooltip.gameObject.SetActive(true);
        }

        public static void Hide() {
            instance.tooltip.gameObject.SetActive(false);
        }

    }

}