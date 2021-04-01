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
        Character characterOwner;
        Pokemon pokemon;

        List<Vector2> movementPattern = new List<Vector2>();

        Vector2 lastMovementPattern;
        float timeBetweenPattern = 0f;
        float idleTimer = 0f;
        int currentPattern = 0;

        public Character CharacterOwner {
            get { return characterOwner; }
        }

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

            if (character.IsMounted) {
                characterOwner.IsMoving = true;
            }

            yield return character.Move(movementPattern[0]);
            movementPattern.RemoveAt(0);

            character.LookTowards(owner.transform.position);

            if (character.IsMounted) {
                characterOwner.IsMoving = false;
            }

            state = PokemonState.Idle;
        }

        public void Interact(Transform initiator) {
            if (state == PokemonState.Idle) {
                character.LookTowards(initiator.position);

                Mount();
            }
        }

        public void Assign(Character characterOwner, Pokemon pokemon) {
            this.characterOwner = characterOwner;
            this.pokemon = pokemon;

            var characterAnimator = GetComponent<CharacterAnimator>();
            var pokemonParty = owner.GetComponent<PokemonParty>();
            var pokemon = pokemonParty.GetHealthyPokemon();

            characterAnimator.WalkDownSprites = pokemon.Base.WalkDownSprites;
            characterAnimator.WalkUpSprites = pokemon.Base.WalkUpSprites;
            characterAnimator.WalkRightSprites = pokemon.Base.WalkRightSprites;
            characterAnimator.WalkLeftSprites = pokemon.Base.WalkLeftSprites;

            RefreshPokemon();
            characterOwner.OnMove += (Vector2 input) => {
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

        public void Mount() {
            characterOwner.IsMounted = true;
            character.IsMounted = true;

            transform.position = characterOwner.transform.position;
            characterOwner.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0f, 0f);

            character.MoveSpeed = 10f;
        }

        public void Unmount() {
            transform.position = character.LastPosition;
            characterOwner.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1f, 1f);

            character.MoveSpeed = 5f;

            characterOwner.IsMounted = false;
            character.IsMounted = false;
        }

        public void AddPattern(Vector2 input) {
            movementPattern.Add(input);
        }

        private void RefreshPokemon() {
            CharacterAnimator characterAnimator = GetComponent<CharacterAnimator>();

            characterAnimator.ChangeCharacterSprites(
                pokemon.Base.WalkDownSprites,
                pokemon.Base.WalkUpSprites,
                pokemon.Base.WalkLeftSprites,
                pokemon.Base.WalkRightSprites
            );

            float pokemonSize = pokemon.Base.GetPokemonSizeMultiplier(true);
            gameObject.transform.localScale = new Vector3(pokemonSize, pokemonSize, 1f); 
        }

    }

    public enum PokemonState {
        Idle,
        Walking,
        Interaction
    }

}