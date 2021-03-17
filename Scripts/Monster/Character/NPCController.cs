using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;
using UI;

namespace Monster.Character {

    public class NPCController : MonoBehaviour, Interactable {

        [SerializeField] Dialog dialog;
        [SerializeField] float timeBetweenPattern;
        [SerializeField] List<Vector2> movementPattern;

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

        IEnumerator Walk() {
            state = NPCState.Walking;

            yield return character.Move(movementPattern[currentPattern]);
            currentPattern = (currentPattern + 1) % movementPattern.Count;

            state = NPCState.Idle;
        }

        public void Interact() {
            if (state == NPCState.Idle) {
                state = NPCState.Dialog;
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => {
                    idleTimer = 0f;
                    state = NPCState.Idle;
                }));
            }
        }

    }

    public enum NPCState {
        Idle,
        Walking,
        Dialog
    }

}