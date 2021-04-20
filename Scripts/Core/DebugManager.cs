using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;
using Core;
using Items;
using Monster.Characters;
using Monster.Outfits;
using Map;
using Map.Tile;

namespace Core.Admin {
    
    public class DebugManager : MonoBehaviour {

        public static DebugManager Instance { get; private set; }

        bool showConsole;
        bool showHelp;
        string input;

        public static DebugCommand KILL_ALL;
        public static DebugCommand<string> ADD_ITEM;
        public static DebugCommand<string> SET_OUTFIT;
        public static DebugCommand<string> ADD_OUTFIT;
        public static DebugCommand<string, string> DROP_ITEM;
        public static DebugCommand<string, string> CHANGE_TILE;
        public static DebugCommand HELP;

        public List<object> commandList;

        Vector2 scroll;

        private void Awake() {
            Instance = this;

            HELP = new DebugCommand("help", "Shows a list of commands", "help", () => {
                showHelp = true;
            });

            LoadCommandItems();
            LoadCommandOutfits();
            LoadCommandRemoves();
            LoadCommmandTiles();

            commandList = new List<object> {
                KILL_ALL,
                ADD_ITEM,
                DROP_ITEM,
                SET_OUTFIT,
                ADD_OUTFIT,
                CHANGE_TILE,
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
                    } else if (commandList[i] as DebugCommand<string, string> != null) {
                        (commandList[i] as DebugCommand<string, string>).Invoke(properties[1], properties[2]);
                    }
                }
            }
        }

        private void LoadCommandRemoves() {
            KILL_ALL = new DebugCommand("kill_all", "Removes all NPCs from the scene.", "kill_all", () => {
                SpawnManager.Instance.KillAllNpcs();
            });
        }

        private void LoadCommandItems() {
            ADD_ITEM = new DebugCommand<string>("add_item", "Sets item to player inventory", "add_item <item_name>", (itemName) => {
                Inventory playerInventory = GameController.Instance.PlayerController.gameObject.GetComponent<Inventory>();

                ItemBase itemBase = (ItemBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Item/{ itemName }.asset", typeof(ItemBase));

                playerInventory.AddItem(new Item(itemBase, 1));
            });

            DROP_ITEM = new DebugCommand<string, string>("drop_item", "Drop an item in a optional place or in front of the command caller", 
                "drop_item <item_name> <world_space>(optional)", (itemName, worldSpace) => {
                    ItemBase itemBase = (ItemBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Item/{ itemName }.asset", typeof(ItemBase));

                    SpawnManager.Instance.SpawnItemInWorld(new Item(itemBase, 1), 
                        GameController.Instance.PlayerController.gameObject.GetComponent<Character>().GetFrontCoordinates());
                });
        }

        private void LoadCommandOutfits() {
            SET_OUTFIT = new DebugCommand<string>("set_outfit", "Sets outfit to player", "set_outfit <outfit_name>", (outfitName) => {
                Character playerCharacter = GameController.Instance.PlayerController.gameObject.GetComponent<Character>();
                
                OutfitBase outfitBase = (OutfitBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Outfit/{ outfitName }.asset", typeof(OutfitBase));

                playerCharacter.ChangeSprites(outfitBase);
            });

            ADD_OUTFIT = new DebugCommand<string>("add_outfit", "Add outfit to player", "add_outfit <outfit_name>", (outfitName) => {
                OutfitInventory playerOutfitInventory = GameController.Instance.PlayerController.gameObject.GetComponent<OutfitInventory>();
                
                OutfitBase outfitBase = (OutfitBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Outfit/{ outfitName }.asset", typeof(OutfitBase));

                playerOutfitInventory.AddOutfit(outfitBase);
            });
        }

        private void LoadCommmandTiles() {
            CHANGE_TILE = new DebugCommand<string, string>("change_tile", "Change tile in front of the command caller", 
                "change_tile <tile_group> <tile_index>", (tileGroup, tileIndex) => {
                    TileDataBase tileDataBase = (TileDataBase) AssetDatabase.LoadAssetAtPath($"Assets/Resources/Map/Tile/{ tileGroup }.asset", typeof(TileDataBase));

                    TileBase tile = tileDataBase.tiles[int.Parse(tileIndex)];

                    

                    MapManager.Instance.IntantiateTileInPosition(tile, 
                        GameController.Instance.PlayerController.gameObject.GetComponent<Character>().GetFrontCoordinates());
                });
        }

    }

}