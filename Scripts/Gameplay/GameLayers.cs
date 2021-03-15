using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Map {

    public class GameLayers : MonoBehaviour {
        
        [SerializeField] LayerMask solidObjectsLayer;
        [SerializeField] LayerMask interactableLayer;
        [SerializeField] LayerMask longGrassLayer;

        public static GameLayers i { get; set; }

        private void Awake() {
            i = this;
        }

        public LayerMask SolidLayer {
            get => solidObjectsLayer;
        }

        public LayerMask InteractableLayer {
            get => interactableLayer;
        }

        public LayerMask LongGrassLayer {
            get => longGrassLayer;
        }

    }

}