using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Monster.Creature;

namespace Map {

    public class GameLayers : MonoBehaviour {
        
        [SerializeField] LayerMask solidObjectsLayer;
        [SerializeField] LayerMask interactableLayer;
        [SerializeField] LayerMask longGrassLayer;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] LayerMask itemWorldLayer;
        [SerializeField] LayerMask pokemonLayer;
        [SerializeField] LayerMask portalLayer;
        [SerializeField] string interfaceSortingLayerName;
        [SerializeField] string instantiatedTileSortingLayerName;

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

        public LayerMask PlayerLayer {
            get => playerLayer;
        }

        public LayerMask ItemWorldLayer {
            get => itemWorldLayer;
        }

        public LayerMask PokemonLayer {
            get => pokemonLayer;
        }

        public LayerMask PortalLayer {
            get => portalLayer;
        }

        public LayerMask TriggerableLayers {
            get => LongGrassLayer | portalLayer;
        }

        public string InterfaceSortingLayerName {
            get => interfaceSortingLayerName;
        }

        public string InstantiatedTileSortingLayerName {
            get => instantiatedTileSortingLayerName;
        }

        public LayerMask DraggableLayers {
            get => LongGrassLayer | interactableLayer | playerLayer | itemWorldLayer | 
                pokemonLayer;
        }

    }

}