using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;
using Map;

namespace Monster.Characters {

    public class Character : MonoBehaviour {

        public delegate void PlayerMoveCallback (Vector2 input);
        public event PlayerMoveCallback OnMove;

        public float moveSpeed;

        private CharacterAnimator animator;

        public bool IsMoving { get; private set; }

        [SerializeField] public Rect Box;

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

            if (!IsPathClear(targetPos)) {
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
            OnMove?.Invoke(moveVec);
        }

        public IEnumerator MoveTo(Vector2 movePos) {
            yield return null;
        }

        public void HandleUpdate() {
            animator.IsMoving = IsMoving;
        }

        public void LookTowards(Vector3 targetPos) {
            var xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
            var yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

            if (xDiff == 0 || yDiff == 0) {
                animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
                animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
            } else {
                Debug.LogError("Error in Look Towards: You can't ask the character to look diagonally");
            }
        }

        private bool IsPathClear(Vector3 targetPos) {
            var diff = targetPos - transform.position;
            var dir = diff.normalized;
            var collOffset = GetComponent<BoxCollider2D>().offset;

            if (Physics2D.BoxCast(transform.position + dir + (Vector3) collOffset, new Vector2(0.2f, 0.2f), 0f, 
                dir, diff.magnitude - 1f, GameLayers.i.SolidLayer | 
                GameLayers.i.InteractableLayer | GameLayers.i.PlayerLayer) == true) {
                return false;
            }

            return true;
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

        private void OnDrawGizmos() {
            var fw = transform.TransformDirection(Box.position);

            Gizmos.color = Color.magenta;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + fw, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, Box.size);
        }

    }

}