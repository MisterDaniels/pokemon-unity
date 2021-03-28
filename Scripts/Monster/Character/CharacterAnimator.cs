﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Monster.Characters {

    public class CharacterAnimator : MonoBehaviour {

        [SerializeField] List<Sprite> walkDownSprites;
        [SerializeField] List<Sprite> walkUpSprites;
        [SerializeField] List<Sprite> walkRightSprites;
        [SerializeField] List<Sprite> walkLeftSprites;

        public List<Sprite> WalkDownSprites { 
            set {
                walkDownSprites = value;
            }
        }
        
        public List<Sprite> WalkUpSprites { 
            set {
                walkUpSprites = value;
            }
        }

        public List<Sprite> WalkRightSprites { 
            set {
                walkRightSprites = value;
            }
        }

        public List<Sprite> WalkLeftSprites { 
            set {
                walkLeftSprites = value;
            }
        }

        // Parameters
        public float MoveX { get; set; }
        public float MoveY { get; set; }
        public bool IsMoving { get; set; }

        // States
        SpriteAnimator walkDownAnim;
        SpriteAnimator walkUpAnim;
        SpriteAnimator walkRightAnim;
        SpriteAnimator walkLeftAnim;

        SpriteAnimator currentAnim;
        bool wasPreviouslyMoving;

        // References
        SpriteRenderer spriteRenderer;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();

            walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
            walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
            walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
            walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

            currentAnim = walkDownAnim;
        }

        private void Update() {
            var prevAnim = currentAnim;

            if (MoveX == 1) {
                currentAnim = walkRightAnim;
            } else if (MoveX == -1) {
                currentAnim = walkLeftAnim;
            } else if (MoveY == 1) {
                currentAnim = walkUpAnim;
            } else if (MoveY == -1) {
                currentAnim = walkDownAnim;
            }

            if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving) {
                currentAnim.Start();
            }

            if (IsMoving) {
                currentAnim.HandleUpdate();
            } else {
                spriteRenderer.sprite = currentAnim.Frames[0];
            }

            wasPreviouslyMoving = IsMoving;
        }

    }

}