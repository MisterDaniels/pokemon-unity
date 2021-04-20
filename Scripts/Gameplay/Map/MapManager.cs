using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Map.Tile;

namespace Map {

    public class MapManager : MonoBehaviour {

        [SerializeField]
        private List<TileDataBase> tileDataBases;

        private Dictionary<TileBase, TileDataBase> dataFromTiles;
        private Grid gridmap;
        private Tilemap[] tilemaps;

        public static MapManager Instance { get; private set; }

        private void Awake() {
            GameObject grid = GameObject.Find("Grid");
            gridmap = grid.GetComponent<Grid>();
            tilemaps = grid.GetComponentsInChildren<Tilemap>(true);

            dataFromTiles = new Dictionary<TileBase, TileDataBase>();

            foreach(var tileDataBase in tileDataBases) {
                foreach(var tile in tileDataBase.tiles) {
                    dataFromTiles.Add(tile, tileDataBase);
                }
            }

            Instance = this;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                TileBase frontTile = GetFrontTile(mousePosition);

                float walkingSpeed = GetTileWalkingSpeed(mousePosition);
            }
        }

        public float GetTileWalkingSpeed(Vector2 worldPosition) {
            TileBase frontTile = GetFrontTile(worldPosition);

            if (frontTile == null) {
                return 1;
            }

            return dataFromTiles[frontTile].walkingSpeed;
        }

        public void IntantiateTileInPosition(TileBase tile, Vector2 worldPosition) {
            foreach(Tilemap tilemap in tilemaps) {
                string tilemapSortingLayerName = tilemap.gameObject.GetComponent<TilemapRenderer>().sortingLayerName;

                if (tilemapSortingLayerName == GameLayers.i.InstantiatedTileSortingLayerName) {
                    Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
                    tilemap.SetTile(gridPosition, tile);
                    break;
                }
            }
        }

        private TileBase GetFrontTile(Vector2 worldPosition) {
            TileBase frontTile = null;
            TilemapRenderer.SortOrder frontSortOrder = 0;
            foreach(Tilemap tilemap in tilemaps) {
                Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
                TileBase tilemapIn = tilemap.GetTile(gridPosition);
                var tilemapSortOrder = tilemap.gameObject.GetComponent<TilemapRenderer>().sortOrder;

                if (tilemapIn && frontSortOrder <= tilemapSortOrder) {
                    frontTile = tilemapIn;
                    frontSortOrder = tilemapSortOrder;
                }
            }

            if (frontTile != null && !dataFromTiles.ContainsKey(frontTile)) {
                return null;
            }

            return frontTile;
        }

    }

}