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

        public CharacterAnimator Animator {
            get => animator;
        }

        private void Awake() {
            animator = GetComponent<CharacterAnimator>();
        }

        public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null) {
            animator.MoveX = moveVec.x;
            animator.MoveY = moveVec.y;

            var targetPos = transform.position;
            targetPos.x += moveVec.x;
            targetPos.y += moveVec.y;

            if (!IsWalkable(targetPos)) {
                yield break;
            }

            animator.IsMoving = true;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                    moveSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPos;
            animator.IsMoving = false;

            OnMoveOver?.Invoke();
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