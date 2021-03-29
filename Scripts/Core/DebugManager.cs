using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Core;
using Items;
using Monster.Characters;

namespace Core.Admin {
    
    public class DebugManager : MonoBehaviour {

        public static DebugManager Instance { get; private set; }

        bool showConsole;
        bool showHelp;
        string input;

        public static DebugCommand KILL_ALL;
        public static DebugCommand<string> ADD_ITEM;
        public static DebugCommand HELP;

        public List<object> commandList;

        Vector2 scroll;

        private void Awake() {
            Instance = this;

            KILL_ALL = new DebugCommand("kill_all", "Removes all NPCs from the scene.", "kill_all", () => {
                SpawnManager.Instance.KillAllNpcs();
            });

            ADD_ITEM = new DebugCommand<string>("add_item", "Sets item to player inventory", "add_item <itemName>", (itemName) => {
                Inventory playerInventory = GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>();

                ItemBase itemBase = (ItemBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Item/{ itemName }.asset", typeof(ItemBase));

                playerInventory.AddItem(new Item(itemBase, 1));
            });

            HELP = new DebugCommand("help", "Shows a list of commands", "help", () => {
                showHelp = true;
            });

            commandList = new List<object> {
                KILL_ALL,
                ADD_ITEM,
                HELP
            };
        }

        public void ToggleConsole() {
            showConsole = !showConsole;
        }

        public void HandleUpdate() {
            if (Input.GetKeyDown(KeyCode.Quote)) {
                GameController.Instance.ToggleConsole();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                HandleInput();
                input = "";
            }
        }

        private void OnGUI() {
            if (!showConsole) { return; }

            float y = 0f;

            if (showHelp) {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");

                Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

                for (int i = 0; i < commandList.Count; i++) {
                    DebugCommandBase command = commandList[i] as DebugCommandBase;

                    string label = $"{ command.CommandFormat } - { command.CommandDescription }";

                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                    GUI.Label(labelRect, label);
                }

                GUI.EndScrollView();

                y += 100;
            }

            GUI.Box(new Rect(0, y, Screen.width, 30), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        }

        private void HandleInput() {
            string[] properties = input.Split(' ');

            for (int i = 0; i < commandList.Count; i++) {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

                if (input.Contains(commandBase.CommandId)) {
                    if (commandList[i] as DebugCommand != null) {
                        (commandList[i] as DebugCommand).Invoke();
                    } else if (commandList[i] as DebugCommand<string> != null) {
                        (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                    }
                }
            }
        }

    }

}