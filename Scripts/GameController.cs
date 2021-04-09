using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;
using Monster.Characters;
using UI.Battle;
using Map;
using UI;
using UI.Menus;
using Core.Admin;

namespace Core {

    public enum GameState {
        FreeRoam,
        Battle,
        Dialog,
        Menu,
        Paused,
        Debug
    }

    public class GameController : MonoBehaviour {
        
        [SerializeField] PlayerController playerController;
        [SerializeField] BattleSystem battleSystem;
        [SerializeField] Camera worldCamera;
        [SerializeField] GameObject canvas;
        
        public static GameController Instance { get; private set; }

        GameState state;
        GameState stateBefore;

        public PlayerController PlayerController => playerController;
        public GameObject Canvas => canvas;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            battleSystem.OnBattleOver += EndBattle;

            DialogManager.Instance.OnShowDialog += () => {
                state = GameState.Dialog;
            };

            DialogManager.Instance.OnCloseDialog += () => {
                if (state == GameState.Dialog) {
                    state = GameState.FreeRoam;
                }
            };

            MenuManager.Instance.OnShowMenu += () => {
                state = GameState.Menu;
            };

            MenuManager.Instance.OnCloseMenu += () => {
                if (state == GameState.Menu) {
                    state = GameState.FreeRoam;
                }
            };
        }

        public void PauseGame(bool pause) {
            if (pause) {
                stateBefore = state;
                state = GameState.Paused;
            } else {
                state = stateBefore;
            }
        }

        public void ToggleConsole() {
            DebugManager.Instance.ToggleConsole();

            if (state != GameState.Debug) {
                stateBefore = state;
                state = GameState.Debug;
            } else {
                state = stateBefore;
            }
        }

        public void StartBattle() {
            state = GameState.Battle;
            battleSystem.gameObject.SetActive(true);
            worldCamera.gameObject.SetActive(false);

            var playerParty = playerController.GetComponent<PokemonParty>();
            var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

            battleSystem.StartBattle(playerParty, wildPokemon);
        }

        private void EndBattle(bool won) {
            state = GameState.FreeRoam;
            battleSystem.gameObject.SetActive(false);
            worldCamera.gameObject.SetActive(true);
        }

        private void Update() {
            if (state == GameState.FreeRoam) {
                playerController.HandleUpdate();
            } else if (state == GameState.Battle) {
                battleSystem.HandleUpdate();
            } else if (state == GameState.Dialog) {
                DialogManager.Instance.HandleUpdate();
            } else if (state == GameState.Menu) {
                MenuManager.Instance.HandleUpdate();
            } else if (state == GameState.Debug) {
                DebugManager.Instance.HandleUpdate();
            }
        }
    }

}