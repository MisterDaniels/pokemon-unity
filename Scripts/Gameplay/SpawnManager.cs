using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;
using Util;
using Monster.Characters;

namespace Core {

    public class SpawnManager : MonoBehaviour {

        public static SpawnManager Instance { get; private set; }
        
        private void Awake() {
            Instance = this;
        }

        public void SpawnItemInWorld(Item item, Vector2 position) {
            Transform transform = Instantiate(PrefabsReference.Instance.ItemOverworld.transform,
                position, Quaternion.identity);

            ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
            itemWorld.SetItem(item);
        }

        public void KillAllNpcs() {
            NPCController [] npcs = FindObjectsOfTypeAll(typeof(NPCController)) as NPCController[];

            foreach (NPCController npc in npcs) {
                Destroy(npc.gameObject);
            }
        }

    }

}