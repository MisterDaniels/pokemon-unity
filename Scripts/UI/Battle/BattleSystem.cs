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
            
            var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
            yield return enemyHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted) {
                yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } Fainted");
            } else {
                StartCoroutine(EnemyMove());
            }
        }

        IEnumerator EnemyMove() {
            state = BattleState.EnemyMove;

            var move = enemyUnit.Pokemon.GetRandomMove();

            yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
            yield return playerHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted) {
                yield return dialogBox.TypeDialog($"{ playerUnit.Pokemon.Base.Name } Fainted");
            } else {
                PlayerAction();
            }
        }

        private IEnumerator ShowDamageDetails(DamageDetails damageDetails) {
            if (damageDetails.Critical > 1f) {
                yield return dialogBox.TypeDialog("A critical hit!");
            }

            if (damageDetails.TypeEffectiveness > 1f) {
                yield return dialogBox.TypeDialog("It's super effective!");
            } else if (damageDetails.TypeEffectiveness < 1f) {
                yield return dialogBox.TypeDialog("It's not effective!");
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