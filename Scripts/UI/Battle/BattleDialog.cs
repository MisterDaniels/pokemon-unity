using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature.Moves;
using UI;

namespace UI.Battle {

    public class BattleDialog : MonoBehaviour {

        [SerializeField] int lettersPerSecond = 30;

        [SerializeField] Text dialogText;
        [SerializeField] GameObject actionSelector;
        [SerializeField] GameObject moveSelector;

        [SerializeField] List<Text> actionTexts;
        [SerializeField] List<GameObject> moveObjects;

        public void SetDialog(string dialog) {
            dialogText.text = dialog;
        }

        public IEnumerator TypeDialog(string dialog) {
            dialogText.text = "";
            
            foreach (var letter in dialog.ToCharArray()) {
                dialogText.text += letter;
                yield return new WaitForSeconds(1f/lettersPerSecond);
            }
            
            yield return new WaitForSeconds(1f);
        }

        public void EnableDialogText(bool enabled) {
            dialogText.enabled = enabled;
        }

        public void EnableActionSelector(bool enabled) {
            actionSelector.SetActive(enabled);
        }

        public void EnableMoveSelector(bool enabled) {
            moveSelector.SetActive(enabled);
        }

        public void UpdateActionSelection(int selectedAction) {
            for (int i = 0; i < actionTexts.Count; i++) {
                if (i == selectedAction) {
                    actionTexts[i].color = DialogManager.Instance.HightlightedColor;
                } else {
                    actionTexts[i].color = Color.black;
                }
            }
        }

        public void UpdateMoveSelection(int selectedMove) {
            for (int i = 0; i < moveObjects.Count; i++) {
                if (i == selectedMove) {
                    moveObjects[i].GetComponent<Image>().color = DialogManager.Instance.HightlightedColor;
                } else {
                    moveObjects[i].GetComponent<Image>().color = Color.white;
                }
            }
        }

        public void SetMoves(List<Move> moves) {
            for (int i = 0; i < moveObjects.Count; i++) {
                if (i < moves.Count) {
                    var moveNameText = moveObjects[i].transform.Find("Name");
                    var movePPText = moveObjects[i].transform.Find("PP");
                    var moveTypeText = moveObjects[i].transform.Find("Type");

                    if (moveNameText) {
                        moveNameText.GetComponent<Text>().text = moves[i].Base.Name;
                    }

                    if (movePPText) {
                        movePPText.GetComponent<Text>().text = $"PP { moves[i].PP.ToString() }/{ moves[i].Base.PP.ToString() }";
                    }

                    if (moveTypeText) {
                        moveTypeText.GetComponent<Text>().text = moves[i].Base.Type.ToString();
                    }
                }
            }
        }

    }

}