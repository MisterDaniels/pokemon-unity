using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Monster.Creature;
using Monster.Creature.Moves;
using Monster.Creature.Data;

public enum BattleState {
    Start,
    ActionSelection,
    MoveSelection,
    PerformMove,
    Busy,
    PartyScreen,
    BattleOver
}

namespace UI.Battle {

    public class BattleSystem : MonoBehaviour {

        [SerializeField] BattleUnit playerUnit;
        [SerializeField] BattleUnit enemyUnit;
        [SerializeField] BattleDialog dialogBox;
        [SerializeField] PartyScreen partyScreen; 

        public event Action<bool> OnBattleOver;

        BattleState state;
        int currentAction;
        int currentMove;
        int currentMember;

        PokemonParty playerParty;
        Pokemon wildPokemon;

        public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon) {
            this.playerParty = playerParty;
            this.wildPokemon = wildPokemon;

            StartCoroutine(SetupBattle());
        }

        public IEnumerator SetupBattle() {
            playerUnit.Setup(playerParty.GetHealthyPokemon());
            enemyUnit.Setup(wildPokemon);

            partyScreen.Init();

            yield return dialogBox.TypeDialog($"A wild { enemyUnit.Pokemon.Base.Name } appeared");

            ChooseFirstTurn();
        }

        private void ActionSelection() {
            state = BattleState.ActionSelection;
            dialogBox.SetDialog("Choose an action");
            dialogBox.EnableActionSelector(true);
        }

        private void MoveSelection() {
            state = BattleState.MoveSelection;
            dialogBox.EnableActionSelector(false);
            dialogBox.EnableDialogText(false);

            dialogBox.EnableMoveSelector(true);

            dialogBox.SetMoves(playerUnit.Pokemon.Moves);
        }

        IEnumerator PlayerMove() {
            var move = playerUnit.Pokemon.Moves[currentMove];
            
            yield return RunMove(playerUnit, enemyUnit, move);

            if (state == BattleState.PerformMove) {
                StartCoroutine(EnemyMove());
            }
        }

        IEnumerator EnemyMove() {
            var move = enemyUnit.Pokemon.GetRandomMove();
            
            yield return RunMove(enemyUnit, playerUnit, move);

            if (state == BattleState.PerformMove) {
                ActionSelection();
            }
        }

        IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move) {
            bool canRunMove = sourceUnit.Pokemon.OnBeforeMove();

            if (!canRunMove) {
                yield return ShowStatusChanges(sourceUnit.Pokemon);
                yield break;
            }
            yield return ShowStatusChanges(sourceUnit.Pokemon);

            state = BattleState.PerformMove;
            move.PP--;

            yield return dialogBox.TypeDialog($"{ sourceUnit.Pokemon.Base.Name } used { move.Base.Name }");
            
            StartCoroutine(sourceUnit.PlayAttackAnimation());
            targetUnit.PlayHitAnimation();

            if (move.Base.Category == MoveCategory.Status) {
                yield return RunMoveEffects(move, sourceUnit.Pokemon, targetUnit.Pokemon);
            } else {
                var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (targetUnit.Pokemon.HP <= 0) {
                yield return dialogBox.TypeDialog($"{ targetUnit.Pokemon.Base.Name } Fainted");
                targetUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);
                
                CheckForBattleOver(targetUnit);
            }

            // Statuses like brn or psn will hurt the pokemon after the turn
            sourceUnit.Pokemon.OnAfterTurn();
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.UpdateHP();

            if (sourceUnit.Pokemon.HP <= 0) {
                yield return dialogBox.TypeDialog($"{ sourceUnit.Pokemon.Base.Name } Fainted");
                sourceUnit.PlayFaintAnimation();

                yield return new WaitForSeconds(2f);
                
                CheckForBattleOver(sourceUnit);
            }
        }

        IEnumerator ShowStatusChanges(Pokemon pokemon) {
            while (pokemon.StatusChanges.Count > 0) {
                string message = pokemon.StatusChanges.Dequeue();
                yield return dialogBox.TypeDialog(message);
            }
        }

        private void CheckForBattleOver(BattleUnit faintedUnit) {
            if (faintedUnit.IsPlayerUnit) {
                var nextPokemon = playerParty.GetHealthyPokemon();
                if (nextPokemon != null) {
                    OpenPartyScreen();
                } else {
                    BattleOver(false);
                }
            } else {
                BattleOver(true);
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
            if (state == BattleState.ActionSelection) {
                HandleActionSelection();
            } else if (state == BattleState.MoveSelection) {
                HandleMoveSelection();
            } else if (state == BattleState.PartyScreen) {
                HandlePartySelection();
            }
        }

        private void HandleActionSelection() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentAction < dialogBox.ActionTexts.Count - 1) {
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
                    MoveSelection();
                } else if (currentAction == 1) {
                    // Run
                } else if (currentAction == 2) {
                    // Pokemon
                    OpenPartyScreen();
                } else if (currentAction == 3) {
                    // Bag
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
                
                StartCoroutine(PlayerMove());
            } else if (Input.GetKeyDown(KeyCode.X)) {
                dialogBox.EnableMoveSelector(false);
                dialogBox.EnableDialogText(true);
                ActionSelection();
            }
        }

        private void HandlePartySelection() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                if (currentMember < playerParty.Pokemons.Count - 1) {
                    ++currentMember;
                }
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                if (currentMember > 0) {
                    --currentMember;
                }
            }

            partyScreen.UpdateMemberSelection(currentMember);

            if (Input.GetKeyDown(KeyCode.Z)) {
                Pokemon selectedPokemon = playerParty.Pokemons[currentMember];

                if (selectedPokemon.HP <= 0) {
                    dialogBox.SetDialog("You can't send out a fainted pokemon");
                    return;
                }

                if (selectedPokemon == playerUnit.Pokemon) {
                    dialogBox.SetDialog("You can't switch with the same pokemon");
                    return;
                }

                partyScreen.gameObject.SetActive(false);
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedPokemon));
            } else if (Input.GetKeyDown(KeyCode.X)) {
                partyScreen.gameObject.SetActive(false);
                ActionSelection();
            }
        }

        private void OpenPartyScreen() {
            state = BattleState.PartyScreen;
            partyScreen.SetPartyData(playerParty.Pokemons);
            partyScreen.gameObject.SetActive(true);
        }

        IEnumerator SwitchPokemon(Pokemon newPokemon) {
            bool currentPokemonFainted = true;

            if (playerUnit.Pokemon.HP > 0) {
                currentPokemonFainted = false;
                
                yield return dialogBox.TypeDialog($"Come back {playerUnit.Pokemon.Base.Name}");
                playerUnit.PlayFaintAnimation();
                yield return new WaitForSeconds(2f);
            }

            playerUnit.Setup(newPokemon);

            dialogBox.SetMoves(newPokemon.Moves);

            yield return dialogBox.TypeDialog($"Go { newPokemon.Base.Name }!");

            if (currentPokemonFainted) {
                ChooseFirstTurn();
            } else {
                StartCoroutine(EnemyMove());
            }
        }

        IEnumerator RunMoveEffects(Move move, Pokemon source, Pokemon target) {
            MoveEffects effects = move.Base.Effects;

            // Stat Boosting
            if (effects.Boosts != null) {
                if (move.Base.Target == MoveTarget.Self) {
                    source.ApplyBoosts(effects.Boosts);
                } else {
                    target.ApplyBoosts(effects.Boosts);
                }
            }

            // Status Condition
            if (effects.Status != ConditionID.none) {
                target.SetStatus(effects.Status);
            }

            yield return ShowStatusChanges(source);
            yield return ShowStatusChanges(target);
        }

        private void BattleOver(bool won) {
            state = BattleState.BattleOver;

            playerParty.Pokemons.ForEach(pokemon => pokemon.OnBattleOver());

            OnBattleOver(won);
        }

        private void ChooseFirstTurn() {
            if (playerUnit.Pokemon.Speed >= enemyUnit.Pokemon.Speed) {
                ActionSelection();
            } else {
                StartCoroutine(EnemyMove());
            }
        }

    }

}