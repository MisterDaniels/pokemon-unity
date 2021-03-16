using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;
using Map;

namespace Monster.Character {

    public class Character : MonoBehaviour {

        public float moveSpeed;

        private CharacterAnimator animator;

        public bool IsMoving { get; private set; }

        public CharacterAnimator Animator {
            get => animator;
        }

        private void Awake() {
            animator = GetComponent<CharacterAnimator>();
        }

        public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null) {
            animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
            animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

            var targetPos = transform.position;
            targetPos.x += moveVec.x;
            targetPos.y += moveVec.y;

            if (!IsWalkable(targetPos)) {
                yield break;
            }

            IsMoving = true;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;

            IsMoving = false;

            OnMoveOver?.Invoke();
        }

        public void HandleUpdate() {
            animator.IsMoving = IsMoving;
        }

        private bool IsWalkable(Vector3 targetPos) {
            // Get center of the footer base
            targetPos.y -= 0.5f;

            if (Physics2D.OverlapCircle(targetPos, 0.01f, GameLayers.i.SolidLayer | 
                GameLayers.i.InteractableLayer) != null) {
                return false;
            }

            return true;
        }

    }

}