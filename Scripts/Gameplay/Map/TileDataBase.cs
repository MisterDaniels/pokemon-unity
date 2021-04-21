using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map.Tile {

    [CreateAssetMenu(fileName = "Tile", menuName = "Tile/Create", order = 0)]
    public class TileDataBase : ScriptableObject {

        public TileBase[] tiles;

        public float walkingSpeed;
        public bool canWalk;

    }

}