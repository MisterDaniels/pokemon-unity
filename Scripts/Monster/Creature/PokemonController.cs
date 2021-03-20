using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Characters;

namespace Monster.Creature {

    public class PokemonController : MonoBehaviour {
    
        Character character;
        PokemonState state;

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

            state = PokemonState.Idle;
        }

        public void Assign(GameObject owner) {
            var character = owner.GetComponent<Character>();

            character.OnMove += (Vector2 input) => {
                if ((input.x != 0 && lastMovementPattern.y != 0) ||
                    (input.y != 0 && lastMovementPattern.x != 0)) {
                    movementPattern.Add(lastMovementPattern);
                }

                lastMovementPattern = input;
                movementPattern.Add(input);
            };
        }

    }

    public enum PokemonState {
        Idle,
        Walking
    }

}