using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;

namespace Monster.Character {

    public class PlayerController : MonoBehaviour {

        public float moveSpeed;
        public LayerMask solidObjectsLayer;
        public LayerMask interactableLayer;
        public LayerMask longGrassLayer;

        public event Action OnEncountered;

        private bool isMoving;
        private Vector2 input;
        private CharacterAnimator animator;

        void Awake() {
            animator = GetComponent<CharacterAnimator>();
        }

        // Update is called once per frame
        public void HandleUpdate() {
            if (!isMoving) {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                // To not move in diagonal
                if (input.x != 0) input.y = 0;

                if (input != Vector2.zero) {
                    animator.MoveX = input.x;
                    animator.MoveY = input.y;

                    var targetPos = transform.position;
                    targetPos.x += input.x;
                    targetPos.y += input.y;

                    if (IsWalkable(targetPos)) {
                        StartCoroutine(Move(targetPos));
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Z)) {
                Interact();
            }
        }

        IEnumerator Move(Vector3 targetPos) {
            isMoving = true;
            animator.IsMoving = isMoving;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            isMoving = false;
            animator.IsMoving = isMoving;

            CheckForEncounters();
        }

        private bool IsWalkable(Vector3 targetPos) {
            // Get center of the footer base
            targetPos.y -= 0.5f;

            if (Physics2D.OverlapCircle(targetPos, 0.01f, solidObjectsLayer | interactableLayer) != null) {
                return false;
            }

            return true;
        }

        private void CheckForEncounters() {
            if (Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer) != null) {
                if (UnityEngine.Random.Range(1, 101) <= 10) {
                    OnEncountered();
                }
            }
        }

        private void Interact() {
            var facingDir = new Vector3(animator.MoveX, animator.MoveY);
            var interactPos = transform.position + facingDir;

            var collider = Physics2D.OverlapCircle(interactPos, 0.1f, interactableLayer);

            if (collider != null) {
                collider.GetComponent<Interactable>()?.Interact();
            }
        }

        private void OnDrawGizmosSelected() {
            var facingDir = new Vector3(animator.MoveX, animator.MoveY);
            var interactPos = transform.position + facingDir;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, interactPos);
        }

    }

}