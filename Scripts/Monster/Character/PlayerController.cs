using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Mechanic;
using Map;
using Monster.Creature;
using Util;

namespace Monster.Characters {

    public class PlayerController : MonoBehaviour {

        public event Action OnEncountered;

        private Vector2 input;
        private Character character;
        private PokemonParty pokemonParty;
        private Transform pokemonOverworld;

        void Awake() {
            character = GetComponent<Character>();
            pokemonParty = GetComponent<PokemonParty>();
        }

        public void HandleUpdate() {
            if (!character.IsMoving) {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                // To not move in diagonal
                if (input.x != 0) input.y = 0;

                if (input != Vector2.zero) {
                    StartCoroutine(character.Move(input, CheckForEncounters));
                }
            }

            character.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Z)) {
                Interact();
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                ShowPokemon();
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
                collider.GetComponent<Interactable>()?.Interact(transform);
            }
        }
        
        private void ShowPokemon() {
            var pokemon = pokemonParty.GetHealthyPokemon();
            
            if (!pokemonOverworld) {
                pokemonOverworld = Instantiate(PrefabsReference.i.PokemonOverworld.transform, new Vector2(
                    transform.position.x, transform.position.y + 1f), Quaternion.identity);

                var PokemonController = pokemonOverworld.GetComponent<PokemonController>();
                PokemonController.Assign(this.gameObject);
            }
        }

    }

}