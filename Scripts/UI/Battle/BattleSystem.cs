using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Pokemon;

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
        int currentMove;

        private void Start() {
            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle() {
            playerUnit.Setup();
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup();
            enemyHud.SetData(enemyUnit.Pokemon);

            yield return dialogBox.TypeDialog($"A wild { enemyUnit.Pokemon.Base.Name } appeared");
            yield return new WaitForSeconds(1f);

            PlayerAction();
        }

        private void PlayerAction() {
            state = BattleState.PlayerAction;
            StartCoroutine(dialogBox.TypeDialog("Choose and action"));
            dialogBox.EnableActionSelector(true);
        }

        private void PlayerMove() {
            state = BattleState.PlayerMove;
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(false);

            dialogBox.EnableMoveSelector(true);

            dialogBox.SetMoves(playerUnit.Pokemon.Moves);
        }

        IEnumerator PerformPlayerMove() {
            state = BattleState.Busy;

            var move = playerUnit.Pokemon.Moves[currentMove];
            yield return dialogBox.TypeDialog($"{ playerUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            yield return new WaitForSeconds(1f);

            bool isFainted = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
            yield return enemyHud.UpdateHP();

            if (isFainted) {
                yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } Fainted");
            } else {
                StartCoroutine(EnemyMove());
            }
        }

        IEnumerator EnemyMove() {
            state = BattleState.EnemyMove;

            var move = enemyUnit.Pokemon.GetRandomMove();

            yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            yield return new WaitForSeconds(1f);

            bool isFainted = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
            yield return playerHud.UpdateHP();

            if (isFainted) {
                yield return dialogBox.TypeDialog($"{ playerUnit.Pokemon.Base.Name } Fainted");
            } else {
                PlayerAction();
            }
        } 

        private void Update() {
            if (state == BattleState.PlayerAction) {
                HandleActionSelection();
            } else if (state == BattleState.PlayerMove) {
                HandleMoveSelection();
            }
        }

        private void HandleActionSelection() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentAction < 1) {
                    ++currentAction;
                }
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (currentAction > 0) {
                    --currentAction;
                }
            }

            dialogBox.UpdateActionSelection(currentAction);

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (currentAction == 0) {
                    PlayerMove();
                } else if (currentAction == 1) {
                    // Run
                }
            }
        }

        private void HandleMoveSelection() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentMove < playerUnit.Pokemon.Moves.Count - 1) {
                    ++currentMove;
                }
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (currentMove > 0) {
                    --currentMove;
                }
            }

            dialogBox.UpdateMoveSelection(currentMove);

            if (Input.GetKeyDown(KeyCode.Z)) {
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                
                StartCoroutine(PerformPlayerMove());
            }

        }

    }

}