using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pokemon;

public enum BattleState {
    Start,
    PlayerAction,
    PlayerMove,
    EnemyMove,
    Busy
}

namespace UI.Battle {

    public class BattleSystem : MonoBehaviour {

        [SerializeField] BattleUnit playerUnit;
        [SerializeField] BattleUnit enemyUnit;
        [SerializeField] BattleHud playerHud;
        [SerializeField] BattleHud enemyHud;
        [SerializeField] BattleDialog dialogBox;

        BattleState state;
        int currentAction;

        private void Start() {
            StartCoroutine(SetupBattle());
        }

        public IEnumerator setupBattle() {
            playerUnit.Setup();
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup();
            enemyHud.SetData(enemyUnit.Pokemon);

            yield return dialogBox.TypeDialog($"A wild { enemyUnity.Pokemon.Base.Name } appeared");
            yield return new WaitForSeconds(1f);

            PlayerAction();
        }

        private void PlayerAction() {
            state = BattleState.PlayerAction;
            StartCoroutine(dialogBox.TypeDialog('Choose and action'));
            dialogBox.EnableActionSelector(true);
        }

        private void Update() {
            if (state == BattleState.PlayerAction) {
                HandleActionSelection();
            }
        }

        private void HandleActionSelection() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentAction < 1) {
                    ++currentAction;
                } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    if (currentAction > 0) {
                        --currentAction;
                    }
                }
            }

            dialogBox.UpdateActionSelection(currentAction);

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (currentAction == 0) {
                    // Fight
                } else if (currentAction == 1) {
                    // Run
                }
            }
        }

    }

}