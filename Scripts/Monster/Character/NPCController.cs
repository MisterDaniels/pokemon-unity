using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;

namespace Monster.Character {

    public class NPCController : MonoBehaviour, Interactable {

        [SerializeField] List<Vector2> movementPattern;
        [SerializeField] float timeBetweenPattern;

        Character character;
        NPCState state;
        float idleTimer = 0f;
        int currentPattern = 0;

        private void Awake() {
            character = GetComponent<Character>();
        }

        private void Update() {
            if (state == NPCState.Idle) {
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

        IEnummerator Walk() {
            state = NPCState.Walking;

            yield return character.Move(movementPattern[currentPattern]);
            currentPattern = (currentPattern + 1) % movementPattern.Count;

            state = NPCState.Idle;
        }

        public void Interact() {
            StartCoroutine(character.Move(new Vector2(0, 2)));
        }

    }

    public enum NPCState {
        Idle,
        Walking
    }

}