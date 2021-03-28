using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Characters;
using Core.Mechanic;
using UI;

namespace Monster.Creature {

    public class PokemonController : MonoBehaviour, Interactable {
    
        Character character;
        PokemonState state;
        GameObject owner;

        List<Vector2> movementPattern = new List<Vector2>();

        Vector2 lastMovementPattern;
        float timeBetweenPattern = 0f;
        float idleTimer = 0f;
        int currentPattern = 0;

        private void Awake() {
            character = GetComponent<Character>();
        }

        private void Update() {
            if (state == PokemonState.Idle) {
                idleTimer += Time.deltaTime;

                if (idleTimer > timeBetweenPattern) {
                    idleTimer = 0f;

                    if (movementPattern.Count > 0) {
                        StartCoroutine(Walk());
                    }
                }
            }

            character.HandleUpdate();
        }

        IEnumerator Walk() {
            state = PokemonState.Walking;

            yield return character.Move(movementPattern[0]);
            movementPattern.RemoveAt(0);

            character.LookTowards(owner.transform.position);

            state = PokemonState.Idle;
        }

        public void Interact(Transform initiator) {
            if (state == PokemonState.Idle) {
                state = PokemonState.Interaction;

                character.LookTowards(initiator.position);

                Dialog dialog = new Dialog();
                dialog.Lines = new List<string>(new string[] { "Eiiiii" });

                StartCoroutine(DialogManager.Instance.ShowDialog(dialog, (bool action) => {
                    idleTimer = 0f;
                    state = PokemonState.Idle;
                }));
            }
        }

        public void Assign(GameObject owner) {
            this.owner = owner;
            var ownerCharacter = owner.GetComponent<Character>();

            var characterAnimator = GetComponent<CharacterAnimator>();
            var pokemonParty = owner.GetComponent<PokemonParty>();
            var pokemon = pokemonParty.GetHealthyPokemon();

            characterAnimator.WalkDownSprites = pokemon.Base.WalkDownSprites;
            characterAnimator.WalkUpSprites = pokemon.Base.WalkUpSprites;
            characterAnimator.WalkRightSprites = pokemon.Base.WalkRightSprites;
            characterAnimator.WalkLeftSprites = pokemon.Base.WalkLeftSprites;

            ownerCharacter.OnMove += (Vector2 input) => {
                if ((input.x != 0 && lastMovementPattern.y != 0) ||
                    (input.y != 0 && lastMovementPattern.x != 0)) {
                    movementPattern.Add(lastMovementPattern);
                    lastMovementPattern = input;
                    return;
                }

                lastMovementPattern = input;
                movementPattern.Add(input);
            };
        }

    }

    public enum PokemonState {
        Idle,
        Walking,
        Interaction
    }

}