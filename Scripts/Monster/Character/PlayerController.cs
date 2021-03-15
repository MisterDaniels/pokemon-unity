using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;

namespace Monster.Character {

    public class PlayerController : MonoBehaviour {

        public event Action OnEncountered;

        private Vector2 input;
        private Character character;

        void Awake() {
            character = GetComponent<Character>();
        }

        // Update is called once per frame
        public void HandleUpdate() {
            if (!character.Animator.IsMoving) {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                // To not move in diagonal
                if (input.x != 0) input.y = 0;

                if (input != Vector2.zero) {
                    StartCoroutine(character.Move(input, CheckForEncounters));
                }
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                Interact();
            }
        }

        private void CheckForEncounters() {
            if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.LongGrassLayer) != null) {
                if (UnityEngine.Random.Range(1, 101) <= 10) {
                    OnEncountered();
                }
            }
        }

        private void Interact() {
            var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
            var interactPos = transform.position + facingDir;

            var collider = Physics2D.OverlapCircle(interactPos, 0.1f, GameLayers.i.InteractableLayer);

            if (collider != null) {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }

        private void OnDrawGizmosSelected() {
            var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
            var interactPos = transform.position + facingDir;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, interactPos);
        }

    }

}