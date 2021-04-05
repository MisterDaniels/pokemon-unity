using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI {

    [ExecuteInEditMode()]
    public class Tooltip : MonoBehaviour {

        [SerializeField] public TextMeshProUGUI textField;
        [SerializeField] public LayoutElement layoutElement;
        [SerializeField] public int characterWrapLimit;

        private RectTransform rectTransform;

        public void SetText(string text) {
            textField.text = text;

            CheckLayout();
        }

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            if (Application.isEditor) {
                CheckLayout();
            }

            Vector2 position = Input.mousePosition;

            float pivotX = position.x / Screen.width;
            float pivotY = position.y / Screen.height;

            rectTransform.pivot = new Vector2(pivotX, pivotY);
            transform.position = position;
        }

        private void CheckLayout() {
            int textLength = textField.text.Length;

            layoutElement.enabled = (textLength > characterWrapLimit) ? true : false;
        }

    }

}