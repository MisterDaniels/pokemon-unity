using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;
using Map;
using Map.Tile;
using Monster.Creature;
using Monster.Outfits;

namespace Monster.Characters {

    public class Character : GameUtils {

        public delegate void PlayerMoveCallback (Vector2 input);
        public event PlayerMoveCallback OnMove;

        [SerializeField] public float moveSpeed = 5f;

        private CharacterAnimator animator;

        public bool IsMoving { get; set; }
        public bool IsMounted { get; set; }
        public Vector2 LastPosition { get; set; }

        public float OffsetY { get; private set; } = 0.3f;

        [SerializeField] public Rect Box;

        public CharacterAnimator Animator {
            get => animator;
        }

        public float MoveSpeed {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        private void Awake() {
            animator = GetComponent<CharacterAnimator>();
            LastPosition = transform.position;
            SetCharacterPositionAndSnapToTile(transform.position);
        }

        public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null, float speedMultiplier = 1f) {
            animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
            animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

            var targetPos = transform.position;
            targetPos.x += moveVec.x;
            targetPos.y += moveVec.y;

            if (!IsPathClear(targetPos)) {
                yield break;
            }

            TileDataBase tileData = MapManager.Instance.GetTileData(targetPos);

            if (tileData == null || !tileData.canWalk) {
                yield break;
            }

            LastPosition = transform.position;
            IsMoving = true;

            while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
                if (IsMounted) {
                    GetComponent<PokemonController>().CharacterOwner.transform.position = 
                        Vector3.MoveTowards(transform.position, targetPos, (moveSpeed * speedMultiplier)
                        * Time.deltaTime);
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPos, 
                    (moveSpeed * speedMultiplier) * Time.deltaTime);
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

        public void ChangeSprites(OutfitBase outfitBase) {
            animator.WalkDownSprites = outfitBase.WalkDownSprites;
            animator.WalkUpSprites = outfitBase.WalkUpSprites;
            animator.WalkRightSprites = outfitBase.WalkRightSprites;
            animator.WalkLeftSprites = outfitBase.WalkLeftSprites;
        }

        public Vector3 GetTransformWithFooterCalculation() {
            Vector3 position = transform.position;
            position.y -= 0.5f;

            return position;
        }

        public Vector2 GetFrontCoordinates() {
            switch(animator.FacingDirection) {
                case FacingDirection.RIGHT: { 
                    return (Vector2) transform.position + new Vector2(1f, 0f);
                }
                case FacingDirection.LEFT: { 
                    return (Vector2) transform.position + new Vector2(-1f, 0f);
                }
                case FacingDirection.UP: { 
                    return (Vector2) transform.position + new Vector2(0f, 1f);
                }
                case FacingDirection.DOWN:
                default: { 
                    return (Vector2) transform.position + new Vector2(0f, -1f);
                }
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