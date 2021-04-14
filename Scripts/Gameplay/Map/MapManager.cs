using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour {

    private Grid gridmap;
    private Tilemap tilemap;

    private void Awake() {
        GameObject grid = GameObject.Find("Grid");
        gridmap = grid.GetComponent<Grid>();
        tilemap = grid.GetComponent<Tilemap>();
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = gridmap.LocalToCell(mousePosition);

            TileBase clickedTile = tilemap.GetTile(gridPosition);

            print($"At position {gridPosition} there is a {clickedTile}");
        }
    }

}