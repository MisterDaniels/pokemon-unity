using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;

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

        public event Action<bool> OnBattleOver;

        BattleState state;
        int currentAction;
        int currentMove;

        PokemonParty playerParty;
        Pokemon wildPokemon;

        public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon) {
            this.playerParty = playerParty;
            this.wildPokemon = wildPokemon;

            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle() {
            playerUnit.Setup(playerParty.GetHealthyPokemon());
            playerHud.SetData(playerUnit.Pokemon);

            enemyUnit.Setup(wildPokemon);
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
            move.PP--;

            yield return dialogBox.TypeDialog($"{ playerUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            StartCoroutine(playerUnit.PlayAttackAnimation());
            enemyUnit.PlayHitAnimation();

            var damageDetails = enemyUnit.Pokemon.TakeDamage(move, playerUnit.Pokemon);
            yield return enemyHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted) {
                yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } Fainted");
                enemyUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);
                OnBattleOver(true);
            } else {
                StartCoroutine(EnemyMove());
            }
        }

        IEnumerator EnemyMove() {
            state = BattleState.EnemyMove;

            var move = enemyUnit.Pokemon.GetRandomMove();
            move.PP--;

            yield return dialogBox.TypeDialog($"{ enemyUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            StartCoroutine(enemyUnit.PlayAttackAnimation());
            playerUnit.PlayHitAnimation();

            var damageDetails = playerUnit.Pokemon.TakeDamage(move, enemyUnit.Pokemon);
            yield return playerHud.UpdateHP();
            yield return ShowDamageDetails(damageDetails);

            if (damageDetails.Fainted) {
                yield return dialogBox.TypeDialog($"{ playerUnit.Pokemon.Base.Name } Fainted");
                playerUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);

                var nextPokemon = playerParty.GetHealthyPokemon();
                if (nextPokemon != null) {
                    playerUnit.Setup(nextPokemon);
                    playerHud.SetData(nextPokemon);

                    dialogBox.SetMoves(nextPokemon.Moves);

                    yield return dialogBox.TypeDialog($"Go { nextPokemon.Base.Name }!");

                    PlayerAction();
                } else {
                    OnBattleOver(false);
                }
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

        public void HandleUpdate() {
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