using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Util;

namespace Monster.Character {

    public class NPCController : MonoBehaviour, Interactable {

        [SerializeField] List<Sprite> sprites;

        SpriteAnimator spriteAnimator;

        private void Start() {
            spriteAnimator = new SpriteAnimator(sprites, GetComponent<SpriteRenderer>());
            spriteAnimator.Start();
        }

        private void Update() {
            spriteAnimator.HandleUpdate();
        }

        public void Interact() {
            Debug.Log("Intecating with NPC");
        }

    }

}