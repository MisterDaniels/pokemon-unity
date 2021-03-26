using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Mechanic;

namespace UI {

    public class DialogManager : MonoBehaviour {

        [SerializeField] GameObject dialogBox;
        [SerializeField] Text dialogText;
        [SerializeField] int lettersPerSecond;
        [SerializeField] GameObject confirmationBox;
        [SerializeField] List<Text> answerTexts;
        [SerializeField] Color hightlightedColor;

        public event Action OnShowDialog;
        public event Action OnCloseDialog;

        public static DialogManager Instance { get; private set; }
        public bool IsShowing { get; private set; }
        public bool IsAnswering { get; private set; }
        public Color HightlightedColor { 
            get { return hightlightedColor; } 
        }

        Dialog dialog;
        int currentLine = 0;
        bool isTyping;
        Action<bool> onDialogFinished;
        int currentAnswer = 0;
        
        private void Awake() {
            Instance = this;
        }

        public IEnumerator ShowDialog(Dialog dialog, Action<bool> onFinished = null) {
            yield return new WaitForEndOfFrame();

            OnShowDialog?.Invoke();
            
            IsShowing = true;
            this.dialog = dialog;
            onDialogFinished = onFinished;
            
            dialogBox.SetActive(true);
            StartCoroutine(TypeDialog(dialog.Lines[0]));
        }

        public void HandleUpdate() {
            if (IsAnswering) {
                if (Input.GetKeyDown(KeyCode.Z)) {
                    IsAnswering = false;
                    confirmationBox.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    if (currentAnswer < 1) {
                        ++currentAnswer;
                    }
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    if (currentAnswer > 0) {
                        --currentAnswer;
                    }
                }

                UpdateAnswerSelection(currentAnswer);
            }

            if (Input.GetKeyDown(KeyCode.Z) && !isTyping && !IsAnswering) {
                ++currentLine;
                if (currentLine < dialog.Lines.Count) {
                    switch(dialog.Lines[currentLine]) {
                        case "?":
                            confirmationBox.SetActive(true);
                            IsAnswering = true;
                            return;
                    }

                    StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
                } else {
                    currentLine = 0;
                    
                    IsShowing = false;
                    dialogBox.SetActive(false);
                    onDialogFinished?.Invoke(currentAnswer > 0 ? true : false);
                    OnCloseDialog?.Invoke();
                }
            }
        }

        public IEnumerator TypeDialog(string line) {
            isTyping = true;
            dialogText.text = "";
            
            foreach (var letter in line.ToCharArray()) {
                dialogText.text += letter;
                yield return new WaitForSeconds(1f / lettersPerSecond);
            }

            isTyping = false;
        }

        private void UpdateAnswerSelection(int currentAnswer) {
            for (int i = 0; i < answerTexts.Count; i++) {
                if (i == currentAnswer) {
                    answerTexts[i].color = hightlightedColor;
                } else {
                    answerTexts[i].color = Color.black;
                }
            }
        }

    }

}